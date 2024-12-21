using System.Runtime.CompilerServices;

namespace EUtility.ValueEx;

public readonly struct EqualResult
{
    public readonly bool Result { get; }
    private readonly bool _inCall = false;

    internal EqualResult(bool result) : this(result, false) { }
    internal EqualResult(bool result, bool incall)
    {
        this.Result = result;
        this._inCall = incall;
    }

    public EqualResult Not() => new(!Result);
    public EqualResult And(EqualResult result) => new(Result & result.Result, true);
    public EqualResult Or(EqualResult result) => new(Result | result.Result, true);
    public EqualResult Xor(EqualResult result) => new(Result ^ result.Result, true);
    public static EqualResult Combie(params EqualResult[] results)
    {
        EqualResult result = default;
        foreach(var item in results)
        {
            result = result.And(item);
            if (result == false)
                return result;
        }
        return result;
    }

    public static bool operator !(EqualResult equalResult) => equalResult.Not().Result;

    public static bool operator &(EqualResult left, EqualResult right) => left.And(right).Result;

    public static bool operator |(EqualResult left, EqualResult right) => left.Or(right).Result;

    public static bool operator ^(EqualResult left, EqualResult right) => left.Xor(right).Result;

    public static bool operator ==(EqualResult left, EqualResult right) => left & right;

    public static bool operator !=(EqualResult left, EqualResult right) => !(left & right);

    public static bool operator ==(EqualResult left, bool right) => left == right;

    public static bool operator !=(EqualResult left, bool right) => !(left == right);

    public static implicit operator bool(EqualResult result) => result.Result;

    public static implicit operator EqualResult(bool result) => new(result);
}

public class EqualHelper
{
    public static EqualResult TypeEqual(object obj1, object obj2)
    {
        Type t1 = obj1.GetType();
        Type t2 = obj2.GetType();
        return t1 == t2;
    }

    public static EqualResult ValueEqual(object obj1, object obj2)
    {
        EqualResult typeEqual = TypeEqual(obj1, obj2);
        if (!typeEqual)
            return false;

        return obj1.Equals(obj2);
    }

    public static EqualResult CollectionLengthEqual<T>(IEnumerable<T> obj1, IEnumerable<T> obj2)
        => obj1.Count() == obj2.Count();

    public static EqualResult CollectionItemsEqual<T>(IList<T> obj1, IList<T> obj2)
    {
        for (int i = 0; i < obj1.Count(); i++)
        {
            if (!ValueEqual(obj1[i].As<object>(), obj2[i].As<object>()))
                return false;
        }
        return true;
    }

    public static EqualResult CollectionEqual<T>(IList<T> obj1, IList<T> obj2)
    {
        EqualResult lengthEqual = CollectionLengthEqual<T>(obj1, obj2);
        if (!lengthEqual)
            return false;

        EqualResult itemsEqual = CollectionItemsEqual(obj1, obj2);
        if (!itemsEqual)
            return false;

        return true;
    }

    public static EqualResult CollectionEqual(object collect1, object collect2)
    {
        return CollectionEqual(collect1.As<ICollection<object>>(), collect2.As<ICollection<object>>());
    }
}
