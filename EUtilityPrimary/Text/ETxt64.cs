using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.Text;

public static class ETxt
{
    private static char[] _indexmap = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVXYZ123456789apzlckemsnsjfuvencgahcjkvnwjqus137648250dbxysbgsj".ToArray();

    public static string GetETxtFromTexts(string text)
    {
        StringBuilder sb = new();
        foreach(var i in text)
        {
            sb.Append(_indexmap[i-32]);
        }
        return sb.ToString();
    }

    public static string GetTextFromETxt(string text)
    {
        var list = _indexmap.ToList();
        StringBuilder sb = new();
        foreach(var i in text)
        {
            sb.Append((char)list.IndexOf(i)+32);
        }
        return sb.ToString();
    }
}
