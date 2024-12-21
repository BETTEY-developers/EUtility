using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility;

public static class ValueValidation
{
    private static void ThrowException(Exception ex)
    {
        if (ex == null)
        {
            throw new ArgumentNullException();
        }

        throw ex;
    }

    public static bool IsNotNull(object obj, bool throwException = false)
    {
        if (obj != null)
            return true;

        if (throwException)
        {
            ThrowException(new ArgumentNullException(nameof(obj)));
        }

        return false;
    }

    public static bool IsInRange<T>(T obj, T min, T max, bool throwException = false)
        where T : class, IComparable<T>
    {
        if(!IsNotNull(obj))
        {
            if(throwException)
            {
                ThrowException(new ArgumentOutOfRangeException(nameof(obj)));
            }
            return false;
        }

        return obj.CompareTo(min) >= 0 && obj.CompareTo(max) <= 0;
    }

    public static bool IsInRange<T>(T obj, T min, T max, bool throwException = false, int _ = 0)
        where T : struct, IComparable<T>
    {
        if (!IsNotNull(obj))
        {
            if (throwException)
            {
                ThrowException(new ArgumentOutOfRangeException(nameof(obj)));
            }
            return false;
        }

        return obj.CompareTo(min) >= 0 && obj.CompareTo(max) <= 0;
    }
}
