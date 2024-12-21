using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.ValueEx;

public class Void
{
    public static Void MakeVoid()
        => new Void();

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
}

public static class VoidExtension
{
    public static bool IsVoid(this object obj)
    {
        return obj is Void;
    }
}
