using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.RegisterEx.File;

class __FileUtil
{
    private static Dictionary<string, List<OpenWith>> _cache = new();

    public static List<OpenWith>? GetWiths(string extension)
    {
        if(_cache.ContainsKey(extension))
            return _cache[extension];

        List<RegistryKey> keys = new List<RegistryKey>();

        string command = "";

        RegistryKey ext = Registry.ClassesRoot.OpenSubKey(extension);

        if (!ValueValidation.IsNotNull(ext)) return null;

        RegistryKey ext2 = ext.OpenSubKey("OpenWithProgids");
        
        if(ValueValidation.IsNotNull(ext2))
        {
            keys.AddRange(GetWithsKeyFromProgids(ext2));
        }

        var defaults = ext.GetDefaultValue() as string;
        if (defaults != "")
            keys.Add(Registry.ClassesRoot.OpenSubKey(defaults));

        foreach(var k in ext.GetValueNames())
        {
            if(k != "Content Type" && k != "" && k != "PerceivedType")
                keys.Add(Registry.ClassesRoot.OpenSubKey(k));
        }

        List<OpenWith> ow = new List<OpenWith>();

        keys.ForEach(x => ow.Add(OpenWith.InitFactory(x)));

        _cache[extension] = ow;

        return ow;
    }

    public static OpenWith GetOpenWith(string extension, int index = 0)
    {
        if (_cache.ContainsKey(extension))
            return _cache[extension][index];

        return GetWiths(extension)[index];
    }

    public static List<RegistryKey> GetWithsKeyFromProgids(RegistryKey progids)
    {
        List<RegistryKey> rk = new List<RegistryKey>();

        foreach(var k in progids.GetValueNames())
        {
            rk.Add(Registry.ClassesRoot.OpenSubKey(k));
        }

        return rk;
    }
}

public readonly struct OpenWith
{
    private OpenWith(string cmd)
    {
        List<string> arguments = new List<string>
        {
            ""
        };
        Stack<char> symbol = new();

        char[] Symbols = "\"<>()$[]{}'".ToCharArray();

        foreach (var ch in cmd)
        {
            if (Symbols.Contains(ch))
            {
                if (symbol.Count > 0 && symbol.Peek() == ch)
                {
                    symbol.Pop();
                }
                else
                {
                    symbol.Push(ch);
                }
            }
            else if (symbol.Count == 0)
            {
                if (ch == ' ')
                {
                    arguments.Add("");
                    continue;
                }
            }
            arguments[arguments.Count - 1] += ch;
        }

        arguments.ForEach(x => x = x.Trim('"'));

        ExecutableFile = arguments[0];
        Arguments = arguments.ToArray()[1..];
    }

    internal static OpenWith InitFactory(RegistryKey rk)
    {
        var ok = rk.OpenSubKey("Shell\\open\\command");
        if (ok == null)
            ok = rk.OpenSubKey("shell\\open\\command");

        return new(ok.GetDefaultValue() as string);
    }

    public string ExecutableFile { get; }
    public string[] Arguments { get; }
}

public static class FileExtension
{
    public static void Launch(this FileInfo fileInfo)
    {
        OpenWith openwith = __FileUtil.GetOpenWith(fileInfo.Extension);

        Process process = new();
        process.StartInfo.FileName = openwith.ExecutableFile;
        string argstring = string.Join(' ', openwith.Arguments);
        if(argstring.Contains("%1"))
        {
            argstring = argstring.Replace("%1", fileInfo.FullName.Replace('\t',' '));
        }
        else
        {
            argstring += " \"" + fileInfo.FullName.Replace('\t', ' ') + "\"";
        }
        process.StartInfo.Arguments = argstring;
        process.Start();
    }
}
