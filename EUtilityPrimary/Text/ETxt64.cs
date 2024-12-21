using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.Text;

public static class ETxt
{
    private static char[] _indexmap = "1q2w3e4r5t6y7u8i9o!a@s#df%g^h&j*k(l)z-x_c=v+b[n{m},;<:>'?\"/QWERTYUIOPASDFGHJKLZXCVBNM".ToArray();

    public static string GetETxtFromTexts(string text)
    {
        StringBuilder sb = new();
        foreach(var i in text)
        {
            int f = i;
            int over = 0;
            if (f >= 85)
            {
                f = f - 85;
                sb.Append('$');
                sb.Append(_indexmap[f]);
            }
            else
                sb.Append(_indexmap[f]);
        }
        return sb.ToString();
    }

    public static string GetTextFromETxt(string text)
    {
        var list = _indexmap.ToList();
        StringBuilder sb = new();
        bool over = false;
        foreach(var i in text)
        {
            if(i == '$')
            {
                over = true;
            }
            else
            {
                if (over)
                {
                    sb.Append((char)(list.IndexOf(i) + 85));
                    over = false;
                }
                else
                    sb.Append((char)list.IndexOf(i));
            }
        }
        return sb.ToString();
    }
}
