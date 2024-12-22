using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.ValueEx;

public class Void
{
    private string _toString = "void";

    public static Void MakeVoid()
        => new Void();

    public static Void MakeVoid(string description)
        => new Void()
        {
            _toString = description
        };

    private Void() { }

    public override int GetHashCode()
    {
        return -1;
    }

    public override bool Equals(object? obj)
    {
        if (EqualHelper.TypeEqual(this, obj).Not())
            return false;

        return true;
    }

    public override string ToString()
    {
        return _toString;
    }
}

public interface IVoidTestable
{
    public bool IsVoid();
}

public static class VoidExtension
{
    public static bool IsVoid(this object obj)
    {
        return obj is Void;
    }

    public static bool IsVoid(this IVoidTestable obj)
    {
        return obj.IsVoid();
    }
}
