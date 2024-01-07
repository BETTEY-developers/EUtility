using EUtility.RegisterEx;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Win32;

namespace EUtility.WinUI.Controls.Files;

class IconHelper
{
    public static async Task<ImageSource> GetIconFromExtenion(string extenion, string filepath)
    {
        if (!Registry.ClassesRoot.GetSubKeyNames().Contains(extenion))
            return new BitmapImage();

        if(extenion == ".exe")
        {
            using Icon di = Icon.ExtractAssociatedIcon(filepath);
            return await di.ToWinUI3();
        }
        using RegistryKey rk = Registry.ClassesRoot.OpenSubKey(extenion);

        RegistryKey fi = Registry.ClassesRoot.OpenSubKey((rk.GetDefaultValue() as string) + "\\DefaultIcon");

        if(fi == null)
        {
            using RegistryKey owp = rk.OpenSubKey("OpenWithProgids");

            fi = Registry.ClassesRoot.OpenSubKey(owp.GetValueNames()[1] + "\\DefaultIcon");
        }

        var s = (fi.GetDefaultValue() as string).Split(',');
        fi.Close();

        if (s.Length == 1)
        {
            var spp = s[0].Split('.');
            if (spp[1] == "ico")
                return await new Icon(s[0]).ToWinUI3();
            else
                return await Icon.ExtractAssociatedIcon(s[0]).ToWinUI3();
        }
        else
        {
            return await GetIcon(s[0], int.Parse(s[1]));
        }
    }

    public static async Task<ImageSource> GetIcon(string exe, int index)
    {
        using var icon = PInvoke.ExtractIcon(exe, (uint)index);

        using Icon di = (Icon)Icon.FromHandle(icon.DangerousGetHandle()).Clone();
        return await di.ToWinUI3();
    }

    public static async Task<ImageSource> GetImageSourceFromICO(string path)
    {
        using Icon icon = new Icon(path);

        return await icon.ToWinUI3();
    }
}

public static class IconExtension
{
    public static async Task<ImageSource> ToWinUI3(this Icon icon)
    {
        using MemoryStream ms = new();
        icon.Save(ms);

        BitmapImage bi = new();
        await bi.SetSourceAsync(ms.AsRandomAccessStream());
        return bi;
    }
}
