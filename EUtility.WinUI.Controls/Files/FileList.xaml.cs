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
using Windows.System;
using Windows.UI.ViewManagement;
using CommunityToolkit.Mvvm.Input;
using Windows.ApplicationModel.DataTransfer;

using EUtility.RegisterEx;
using EUtility.RegisterEx.File;
using Windows.Services.Maps;
using Windows.Win32;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Microsoft.VisualBasic.FileIO;
using Windows.ApplicationModel.DataTransfer.DragDrop.Core;
using Windows.ApplicationModel.DataTransfer.DragDrop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EUtility.WinUI.Controls.Files;

public class ItemOrderRadio : Helpers.EnumRadio<ItemSortOrder>;

public class ItemSortBaseElementRadio : Helpers.EnumRadio<ItemSortBaseElement>;

public class ItemSortGroupRadio : Helpers.EnumRadio<ItemSortGroup>;

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
        = DependencyProperty.Register("Path", typeof(string), typeof(FileList), new("C:\\\\"));

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

    public bool ShowPrevious
    {
        get 
        { 
            return (bool)GetValue(ShowPreviousProperty); 
        }
        set 
        { 
            SetValue(ShowPreviousProperty, value); 
            OnPropertyChanged(nameof(ShowPrevious));
        }
    }

    // Using a DependencyProperty as the backing store for ShowPrevious.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShowPreviousProperty =
        DependencyProperty.Register("ShowPrevious", typeof(bool), typeof(FileList), new PropertyMetadata(true));



    public bool IsInRoot
    {
        get { return (bool)GetValue(IsInRootProperty); }
        set 
        { 
            SetValue(IsInRootProperty, value); 
            OnPropertyChanged(nameof(IsInRoot));
        }
    }

    // Using a DependencyProperty as the backing store for IsInRoot.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsInRootProperty =
        DependencyProperty.Register("IsInRoot", typeof(bool), typeof(FileList), new PropertyMetadata(false));

    [ObservableProperty]
    private bool _isSelectdItem = false;

    [ObservableProperty]
    private bool _isSelectDirectory = false;

    [ObservableProperty]
    private bool _isSelectFile = false;

    [ObservableProperty]
    private bool _canPaste = false;

    [ObservableProperty]
    public ObservableCollection<DriveItemsData> _driveInfos = new();

    private ListViewItem _rightItem;

    public StorageFile Result => throw new NotImplementedException();

    public bool IsSuccess => throw new NotImplementedException();

    private bool _refresh = false;

    private bool _NonHandle = false;

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        DirectoryInfo di = new(Path);

        DirectoryItems = new();

        DirectoryItems.Add(new(new DirectoryInfo(string.Join('\\', di.FullName.Split('\\')[..^1])), new(), "Previous Folder"));

        foreach (var item in DriveInfo.GetDrives())
        {
            BitmapImage bitmap = default;
            if(item.DriveType is DriveType.Fixed or DriveType.Removable)
            {
                bitmap = (BitmapImage)this.Resources["DriveImage"];
            }
            else if(item.DriveType == DriveType.CDRom)
            {
                bitmap = (BitmapImage)this.Resources["CDImage"];
            }
            else
            {
                continue;
            }
            DriveInfos.Add(new(item, bitmap));
        }

        (await GetSortedDirectoryDatas(di, ItemComparisonFactory(SortOrder, SortBaseElement))).ToList().ForEach(DirectoryItems.Add);

        PropertyChanged += FileList_PropertyChanged;
    }

    #region PropertyChanged

    private async void FileList_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DirectoryItems) || 
            e.PropertyName == nameof(IsSelectdItem) ||
            e.PropertyName == nameof(IsSelectDirectory) ||
            e.PropertyName == nameof(IsSelectFile) ||
            e.PropertyName == nameof(IsInRoot) ||
            _NonHandle)
        {
            _NonHandle = false;
            return;
        }

        DirectoryItems.Clear();
        if(Path.Length == 1)
        {
            IsInRoot = true;
            return;
        }
        else
        {
            IsInRoot = false;
        }

        if(ShowPrevious)
            DirectoryItems.Add(new(new DirectoryInfo(string.Join("", new Uri(Path).Segments[..^1])), new(), "Previous Folder"));

        if(Path.Length > 1)
        {
            _NonHandle = true;
            Path = Path.TrimStart('/');
        }
            

        (await GetSortedDirectoryDatas(new(Path), ItemComparisonFactory(SortOrder, SortBaseElement))).ToList().ForEach(DirectoryItems.Add);
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

    private async Task<ObservableCollection<DirectoryItemsData>> GetSortedDirectoryDatas(DirectoryInfo di, Comparison<FileSystemInfo> comparison)
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
                BitmapImage bi = new();
                try
                {
                    var f = await StorageFile.GetFileFromPathAsync(item.FullName);
                    bi.SetSource((await f.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.MusicView)).AsStream().AsRandomAccessStream());
                }
                catch { }
                
                result.Add(new(item, bi));
            }
            else
            {
                result.Add(new(item, (BitmapImage)this.Resources[(object)"FolderImage"]));
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

    private async void ListViewItem_RightTapped(object sender, RightTappedRoutedEventArgs e)
    {
        IsSelectdItem = false;
        IsSelectDirectory = false;
        IsSelectFile = false;
        var senderconved = ((ListViewItem)sender);
        var dpv = Clipboard.GetContent();
        // Get files
        try
        {
            var items = await dpv.GetStorageItemsAsync();
            if (items.Count > 0)
            {
                CanPaste = true;
            }
        }
        catch { }
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

    [RelayCommand]
    private void Comp()
    {
        var info = ((DirectoryItemsData)_rightItem.DataContext).Info;

        if(info.Attributes.HasFlag(System.IO.FileAttributes.Directory))
        {
            ZipFile.CreateFromDirectory(info.FullName, info.FullName + ".zip");
        }
        else
        {
            var direct = Directory.CreateDirectory(info.Name);
            File.Copy(info.FullName, global::System.IO.Path.Combine(direct.FullName, info.Name + info.Extension));
            ZipFile.CreateFromDirectory(direct.Parent.FullName, direct.FullName);
        }
    }

    [RelayCommand]
    private void CopyPath()
    {
        DataPackage dp = new DataPackage();
        dp.SetText(((DirectoryItemsData)_rightItem.DataContext).Info.FullName);
        Clipboard.SetContent(dp);
    }

    DirectoryItemsData _renameitem = default;

    [RelayCommand]
    private void RenameFile()
    {
        var select = (DirectoryItemsData)ListArea.SelectedItem;
        int index = DirectoryItems.IndexOf(select);
        var newl = DirectoryItems;
        select.IsEditing = true;
        newl[index] = select;
        DirectoryItems = newl;
        _renameitem = select;
    }

    [RelayCommand]
    private async void CopyFile()
    {
        DataPackage dp = new();
        List<StorageFile> files = new();
        foreach (var item in ListArea.SelectedItems)
        {
            files.Add(await StorageFile.GetFileFromPathAsync(((DirectoryItemsData)item).Info.FullName));
        }
        dp.SetStorageItems(files);
        Clipboard.SetContent(dp);
    }

    [RelayCommand]
    private void PasteItem()
    {
        if (!IsSelectdItem)
            PasteItemToCurrent();
    }

    private async void PasteItemToCurrent()
    {
        DataPackageView dpv = Clipboard.GetContent();
        var items = await dpv.GetStorageItemsAsync();

        foreach(var item in items)
        {
            
        }
    }

    private async void ListViewItem_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        if (_refresh)
        {
            _refresh = false;
            return;
        }
        var data = (DirectoryItemsData)ListArea.SelectedItem;

        if(data.DisplayName == "Previous Folder")
        {
            _NonHandle = true;
            Path = string.Join("", new Uri(Path).Segments[..^1]);
            if(Path.Length > 1)
                Path = Path.TrimStart('/');
            else
                Path = Path;
            return;
        }

        if (data.Info.Attributes.HasFlag(System.IO.FileAttributes.Directory))
        {
            _refresh = true;
            Path = data.Info.FullName;
        }
        else
        {
            if(!await Launcher.LaunchFileAsync(await StorageFile.GetFileFromPathAsync(data.Info.FullName)))
            {
                ((FileInfo)data.Info).Launch();
            }
        }
    }

    private void GridViewItem_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        Path = ((DriveItemsData)DriveList.SelectedItems[0]).Info.Name;
    }

    private async void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var select = _renameitem;
        if (((TextBox)sender).Text == select.Info.Name + select.Info.Extension)
            return;
        File.Move(select.Info.FullName, System.IO.Path.Combine(System.IO.Path.GetFullPath(Path), ((TextBox)sender).Text));
        DirectoryItems.Clear();
        if (ShowPrevious)
            DirectoryItems.Add(new(new DirectoryInfo(string.Join("", new Uri(Path).Segments[..^1])), new(), "Previous Folder"));
        (await GetSortedDirectoryDatas(new(Path), ItemComparisonFactory(SortOrder, SortBaseElement))).ToList().ForEach(DirectoryItems.Add);
    }
}

public record struct DirectoryItemsData(FileSystemInfo Info, BitmapImage Icon, string DisplayName = "$Default$", bool IsEditing = false)
{
    public string GetUIName()
    {
        return DisplayName == "$Default$" ? Info.Name : DisplayName;
    }

    private string _name = Info.Name;

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }

    public static implicit operator (FileSystemInfo Info, BitmapImage Icon, string DisplayName)(DirectoryItemsData value)
    {
        return (value.Info, value.Icon, value.DisplayName);
    }

    public static implicit operator DirectoryItemsData((FileSystemInfo info, BitmapImage icon, string DisplayName) value)
    {
        return new DirectoryItemsData(value.info, value.icon, value.DisplayName);
    }

    public ImageSource GetSource()
        => Icon;
}

public partial record struct DriveItemsData(DriveInfo Info, BitmapImage Icon)
{
    public double GetUsedSpace()
    {
        return Info.TotalSize - Info.TotalFreeSpace;
    }

    public string GetDescription()
    {
        return FileSizeConverter.GetSizeStringFromBytesCount(Info.AvailableFreeSpace) +
               " ø…”√£¨π≤" +
               FileSizeConverter.GetSizeStringFromBytesCount(Info.TotalSize);
    }

    public string GetUIName()
    {
        return $"{Info.VolumeLabel} ({Info.Name[..^1]})";
    }
}

internal class ExternConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        FileSystemInfo fileSystemInfo = (value as FileSystemInfo);
        if (fileSystemInfo.Attributes.HasFlag(System.IO.FileAttributes.Directory))
            return "";
        string f = "";
        try
        {
            f = StorageFile.GetFileFromPathAsync(fileSystemInfo.FullName).GetAwaiter().GetResult().DisplayType;
        }
        catch { }
        return f;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class FileSizeConverter : IValueConverter
{
    public static string GetSizeStringFromBytesCount(long size)
    {
        const long KB = 1024;
        const long MB = 1024 * 1024;
        const long GB = 1024 * 1024 * 1024;

        return size switch
        {
            < KB => $"{size}B",
            < MB => $"{size / KB}KB",
            < GB => $"{size / MB}MB",
            >= GB => $"{size / GB}GB"
        };
    }
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if ((value as FileInfo) == null)
            return "";
        return GetSizeStringFromBytesCount((value as FileInfo).Length);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
