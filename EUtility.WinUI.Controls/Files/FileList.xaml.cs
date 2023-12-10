using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using JboxTransfer.Helpers;
using Windows.System;
using Windows.UI.ViewManagement;
using CommunityToolkit.Mvvm.Input;
using Windows.ApplicationModel.DataTransfer;

using EUtility.RegisterEx;
using EUtility.RegisterEx.File;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EUtility.WinUI.Controls.Files;

public enum ItemSortOrder
{
    Ascending,
    Descending
}

public enum ItemSortBaseElement
{
    Name,
    CreatedTime,
    LastWroteTime,
    Size
}

public enum ItemSortGroup
{
    None,
    OnlyFileFolder,
    Type
}

[ObservableObject]
public sealed partial class FileList : UserControl, IResult<StorageFile>
{

    public FileList()
    {
        this.InitializeComponent();
    }

    [ObservableProperty]
    private ObservableCollection<DirectoryItemsData> _directoryItems;

    public static DependencyProperty PathProperty
        = DependencyProperty.Register("Path", typeof(string), typeof(FileList), new("C:\\"));

    public string Path
    {
        get
        {
            return (string)GetValue(PathProperty);
        }
        set
        {
            SetValue(PathProperty, value);
            OnPropertyChanged(nameof(Path));
        }
    }

    public static DependencyProperty ChooseFileTypeProperty
        = DependencyProperty.Register("ChooseFileType", typeof(Dictionary<string, IList<string>>), typeof(FileList), new(new Dictionary<string, IList<string>>()));

    public Dictionary<string, IList<string>> ChooseFileType
    {
        get
        {
            return (Dictionary<string, IList<string>>)GetValue(ChooseFileTypeProperty);
        }
        set
        {
            SetValue(ChooseFileTypeProperty, value);
        }
    }

    public static DependencyProperty SortOrderProperty
         = DependencyProperty.Register("SortOrder", typeof(ItemSortOrder), typeof(FileList), new(ItemSortOrder.Ascending));

    public ItemSortOrder SortOrder
    {
        get
        {
            return (ItemSortOrder)GetValue(SortOrderProperty);
        }
        set
        {
            SetValue(SortOrderProperty, value);
            OnPropertyChanged(nameof(SortOrder));
        }
    }

    public static DependencyProperty SortBaseElementProperty
        = DependencyProperty.Register("SortBaseElement", typeof(ItemSortBaseElement), typeof(FileList), new(ItemSortBaseElement.Name));

    public ItemSortBaseElement SortBaseElement
    {
        get
        {
            return (ItemSortBaseElement)GetValue(SortBaseElementProperty);
        }
        set
        {
            SetValue(SortBaseElementProperty, value);
            OnPropertyChanged(nameof(SortBaseElement));
        }
    }

    public static DependencyProperty SortGroupProperty
        = DependencyProperty.Register("SortGroup", typeof(ItemSortGroup), typeof(FileList), new(ItemSortGroup.None));

    public ItemSortGroup SortGroup
    {
        get
        {
            return (ItemSortGroup)GetValue(SortGroupProperty);
        }
        set
        {
            SetValue(SortGroupProperty, value);
            OnPropertyChanged(nameof(SortGroupProperty));
        }
    }

    public static DependencyProperty ShowGroupProperty
        = DependencyProperty.Register("ShowGroup", typeof(bool), typeof(FileList), new(false));

    public bool ShowGroup
    {
        get
        {
            return (bool)GetValue(ShowGroupProperty);
        }
        set
        {
            SetValue(ShowGroupProperty, value);
            OnPropertyChanged(nameof(ShowGroup));
        }
    }

    [ObservableProperty]
    private bool _isSelectdItem = false;

    [ObservableProperty]
    private bool _isSelectDirectory = false;

    [ObservableProperty]
    private bool _isSelectFile = false;

    private ListViewItem _rightItem;

    public StorageFile Result => throw new NotImplementedException();

    public bool IsSuccess => throw new NotImplementedException();

    public bool _refresh = false;

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        DirectoryInfo di = new(Path);

        DirectoryItems = new();

        DirectoryItems.Add(new(new DirectoryInfo(string.Join('\\', di.FullName.Split('\\')[..^1])), new(), "Previous Folder"));

        GetSortedDirectoryDatas(di, ItemComparisonFactory(SortOrder, SortBaseElement)).ToList().ForEach(DirectoryItems.Add);

        PropertyChanged += FileList_PropertyChanged;
    }

    #region PropertyChanged

    private void FileList_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DirectoryItems) || 
            e.PropertyName == nameof(IsSelectdItem) ||
            e.PropertyName == nameof(IsSelectDirectory) ||
            e.PropertyName == nameof(IsSelectFile))
            return;

        DirectoryItems.Clear();

        DirectoryItems.Add(new(new DirectoryInfo(string.Join('\\', Path.Split('\\')[..^1])), new(), "Previous Folder"));

        GetSortedDirectoryDatas(new(Path), ItemComparisonFactory(SortOrder, SortBaseElement)).ToList().ForEach(DirectoryItems.Add);
    }
    
    #endregion

    private Comparison<FileSystemInfo> ItemComparisonFactory(ItemSortOrder Order, ItemSortBaseElement sort) =>
    (x, y) =>
    {
        FileSystemInfo first = default;
        FileSystemInfo second = default;

        switch (Order)
        {
            case ItemSortOrder.Ascending:
                first = x;
                second = y;
                break;

            case ItemSortOrder.Descending:
                first = y;
                second = x;
                break;
        }

        switch (sort)
        {
            case ItemSortBaseElement.Name:
                return first.Name.CompareTo(second.Name);

            case ItemSortBaseElement.CreatedTime:
                return first.CreationTime.CompareTo(second.CreationTime);

            case ItemSortBaseElement.LastWroteTime:
                return first.LastWriteTime.CompareTo(second.LastWriteTime);

            case ItemSortBaseElement.Size:
                if (first.Attributes.HasFlag(System.IO.FileAttributes.Directory) || second.Attributes.HasFlag(System.IO.FileAttributes.Directory))
                    return -1;
                return new FileInfo(first.FullName).Length.CompareTo(new FileInfo(second.FullName).Length);

            default:
                goto case ItemSortBaseElement.Name;
        }
    };

    private ObservableCollection<DirectoryItemsData> GetSortedDirectoryDatas(DirectoryInfo di, Comparison<FileSystemInfo> comparison)
    {
        const int FILE = 0;
        const int FOLDER = 1;

        List<FileSystemInfo> list = GetDirectoryDatas(di);

        if (SortGroup == ItemSortGroup.None)
        {
            list.Sort(comparison);
        }
        else if(SortGroup == ItemSortGroup.OnlyFileFolder)
        {
            List<FileSystemInfo>[] nomerge = new List<FileSystemInfo>[]
            {
                new(),
                new()
            };

            foreach(var item in list)
            {
                if(item.Attributes.HasFlag(System.IO.FileAttributes.Directory))
                {
                    nomerge[FOLDER].Add(item);
                }
                else
                {
                    nomerge[FILE].Add(item);
                }
            }

            nomerge[FOLDER].Sort(comparison);
            nomerge[FILE].Sort(comparison);

            list = nomerge[FOLDER].Concat(nomerge[FILE]).ToList();
        }
        else
        {
            Dictionary<string, List<FileSystemInfo>> types = new()
            {
                ["Folder"] = new() 
            };

            foreach (var item in list)
            {
                if(item.Attributes.HasFlag(System.IO.FileAttributes.Directory))
                {
                    types["Folder"].Add(item);
                    continue;
                }

                if(!types.ContainsKey(item.Extension))
                {
                    types.Add(item.Extension, new());
                }

                types[item.Extension].Add(item);
            }

            foreach (var type in types)
            {
                type.Value.Sort(comparison);
            }

            var nosorttype = types.Skip(1).ToList();
            nosorttype.Sort((x,y)=>x.Key.CompareTo(y.Key));

            var sorttype = new List<FileSystemInfo>();

            sorttype.AddRange(types["Folder"]);

            nosorttype.ForEach(x => sorttype.AddRange(x.Value));

            list = sorttype;
        }

        ObservableCollection<DirectoryItemsData> result = new();

        foreach (var item in list)
        {
            if ((item as DirectoryInfo) == null)
            {
                result.Add(new(item, (BitmapImage)IconHelper.FindIconForFilename(item.FullName, true)));
            }
            else
            {
                result.Add(new(item, new()));
            }
        }

        return result;
    }

    private static List<FileSystemInfo> GetDirectoryDatas(DirectoryInfo di)
    {
        List<FileSystemInfo> list = new();
        foreach (var item in di.EnumerateFiles())
        {
            list.Add(item);
        }
        foreach (var item in di.EnumerateDirectories())
        {
            list.Add(item);
        }

        return list;
    }

    private void ListViewItem_RightTapped(object sender, RightTappedRoutedEventArgs e)
    {
        IsSelectdItem = false;
        IsSelectDirectory = false;
        IsSelectFile = false;
        var senderconved = ((ListViewItem)sender);
        if (senderconved.IsSelected)
        {
            IsSelectdItem = true;
            _rightItem = (ListViewItem)sender;
            if (!((DirectoryItemsData)senderconved.DataContext).Info.Attributes.HasFlag(System.IO.FileAttributes.Directory))
            {
                IsSelectFile = true;
                FileRightBar.ShowAt((FrameworkElement)sender);
            }
            else
            {
                IsSelectDirectory = true;
                FileRightBar.ShowAt((FrameworkElement)sender);
            }
        }
        else
        {
            IsSelectDirectory = true;
            FileRightBar.ShowAt((FrameworkElement)sender);
        }
        
    }

    /* Commands */

    [RelayCommand]
    private void OpenFile()
    {
        (((DirectoryItemsData)_rightItem.DataContext).Info as FileInfo).Launch();
    }

    private async void ListViewItem_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        if (_refresh)
        {
            _refresh = false;
            return;
        }
        var data = (DirectoryItemsData)ListArea.SelectedItem;

        if (data.Info.Attributes.HasFlag(System.IO.FileAttributes.Directory))
        {
            _refresh = true;
            Path = data.Info.FullName;
        }
        else
        {
            ((FileInfo)data.Info).Launch();
        }
    }
}

public record struct DirectoryItemsData(FileSystemInfo Info, BitmapImage Icon, string DisplayName = "$Default$")
{
    public string GetUIName()
    {
        return DisplayName == "$Default$" ? Info.Name : DisplayName;
    }

    public static implicit operator (FileSystemInfo Info, BitmapImage Icon, string DisplayName)(DirectoryItemsData value)
    {
        return (value.Info, value.Icon, value.DisplayName);
    }

    public static implicit operator DirectoryItemsData((FileSystemInfo info, BitmapImage icon, string DisplayName) value)
    {
        return new DirectoryItemsData(value.info, value.icon, value.DisplayName);
    }
}


internal class ExternConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        string ext = value as string;

        RegistryKey extreg = Registry.ClassesRoot.OpenSubKey(ext);
        if (extreg == null)
        {
            return ext[1..].ToUpper();
        }

        RegistryKey extFriendly = Registry.ClassesRoot.OpenSubKey((extreg.GetValue("") as string)??"");
        if (extFriendly == null)
        {
            extreg.Close();
            return ext[1..].ToUpper();
        }

        string extFriendlyName = extFriendly.GetValue("") as string;

        extFriendly.Close();
        extreg.Close();

        return extFriendlyName;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class FileSizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        const long KB = 1024;
        const long MB = 1024 * 1024;
        const long GB = 1024 * 1024 * 1024;

        if (value.GetType() != typeof(FileInfo))
            return "";

        var size = ((FileInfo)value).Length;

        return size switch
        {
            < KB => $"{size}B",
            < MB => $"{size / KB}KB",
            < GB => $"{size / MB}MB",
            >= GB => $"{size / GB}GB"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
