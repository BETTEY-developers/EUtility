using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.ValueEx;

public static class ObjectExtension
{
    public static T As<T>(this object obj) where T : class
    {
        return (T)obj;
    }

    public static T As<T>(this object obj, int _ = 1) where T : struct
    {
        return (T)obj;
    }
}
