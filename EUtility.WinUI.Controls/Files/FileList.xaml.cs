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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EUtility.WinUI.Controls.Files;

internal partial class FileListViewModel : ObservableRecipient
{
    [ObservableProperty]
    private ObservableCollection<DirectoryItemsData> _directoryItems;
}

public sealed partial class FileList : UserControl, IResult<StorageFile>
{
    FileListViewModel ViewModel { get; set; }

    public FileList()
    {
        ViewModel = new();
        this.InitializeComponent();
    }

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

    public StorageFile Result => throw new NotImplementedException();

    public bool IsSuccess => throw new NotImplementedException();

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        DirectoryInfo di = new(Path);

        ViewModel.DirectoryItems = GetDirectoryDatas(di, (x, y) => x.Name.CompareTo(y.Name));
    }

    private ObservableCollection<DirectoryItemsData> GetDirectoryDatas(DirectoryInfo di, Comparison<FileSystemInfo> comparison)
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

        list.Sort(comparison);

        ObservableCollection<DirectoryItemsData> result = new();

        foreach (var item in list)
        {
            if ((item as DirectoryInfo) == null)
            {
                result.Add((item, (BitmapImage)IconHelper.FindIconForFilename(item.FullName, true)));
            }
        }

        return result;
    }

    private void ListArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
    }
}

internal record struct DirectoryItemsData(FileSystemInfo Info, BitmapImage Icon)
{
    public static implicit operator (FileSystemInfo Info, BitmapImage Icon)(DirectoryItemsData value)
    {
        return (value.Info, value.Icon);
    }

    public static implicit operator DirectoryItemsData((FileSystemInfo info, BitmapImage icon) value)
    {
        return new DirectoryItemsData(value.info, value.icon);
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
