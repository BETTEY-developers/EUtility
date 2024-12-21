namespace EUtility.ValueEx;

public class UnionTypeNoneValueException : Exception
{
    public UnionTypeNoneValueException(Type type) : base($"Getting type [{type}] value is none to set before.") { }
    public UnionTypeNoneValueException() : base($"Value is none to set before.") { }
}

public class Union<UnT1, UnT2> : ICloneable
{
    private UnT1 _T1Value = default(UnT1);
    private UnT2 _T2Value = default(UnT2);

    private bool[] _hasValue = new bool[2];

    public Union(UnT1 value)
    {
        _T1Value = value;
        _T2Value = default(UnT2);

        _hasValue = new bool[2] { true, false };
    }

    public Union(UnT2 value)
    {
        _T2Value = value;
        _T1Value = default(UnT1);

        _hasValue = new bool[2] { false, true };
    }

    private bool CheckT1(out UnT1 PT1)
    {
        if (_hasValue[0])
        {
            PT1 = _T1Value;
            return true;
        }
        PT1 = default;
        return false;
    }

    private bool CheckT2(out UnT2 PT2)
    {
        if (_hasValue[0])
        {
            PT2 = _T2Value;
            return true;
        }
        PT2 = default;
        return false;
    }

    public static explicit operator UnT1(Union<UnT1, UnT2> value)
    {
        UnT1 r;
        if (value.CheckT1(out r))
            return r;

        throw new UnionTypeNoneValueException(typeof(UnT1));
    }

    public static explicit operator UnT2(Union<UnT1, UnT2> value)
    {
        UnT2 r;
        if (value.CheckT2(out r))
            return r;

        throw new UnionTypeNoneValueException(typeof(UnT2));
    }

    public static implicit operator Union<UnT1, UnT2>(UnT1 value)
    {
        return new(value);
    }

    public static implicit operator Union<UnT1, UnT2>(UnT2 value)
    {
        return new(value);
    }

    public static object operator ~(Union<UnT1, UnT2> value)
    {
        object result = new();
        if (value._hasValue[0])
            result = value._T1Value;
        else if (value._hasValue[1])
            result = value._T2Value;
        else
            throw new UnionTypeNoneValueException(typeof(UnT1));

        value._T2Value = default;
        value._T1Value = default;
        value._hasValue = new bool[2] { false, false };
        return result;
    }

    public static Union<UnT1, UnT2> operator |(Union<UnT1, UnT2> obj, UnT1 value)
    {
        obj._T1Value = value;
        obj._T2Value = default(UnT2);

        obj._hasValue = new bool[2] { true, false };
        return obj;
    }

    public static Union<UnT1, UnT2> operator |(Union<UnT1, UnT2> obj, UnT2 value)
    {
        obj._T2Value = value;
        obj._T1Value = default(UnT1);

        obj._hasValue = new bool[2] { false, true };
        return obj;
    }

    public static bool operator ==(Union<UnT1, UnT2> obj1, Union<UnT1, UnT2> obj2)
    {
        bool typeEqual = EqualHelper.TypeEqual(obj1, obj2);
        if (!typeEqual)
            return false;

        bool nonvalueEqual = EqualHelper.CollectionEqual(obj1._hasValue, obj2._hasValue);
        if (!nonvalueEqual)
            return false;

        bool t1valueEqual = EqualHelper.ValueEqual(obj1._T1Value ?? (object)Void.MakeVoid(), obj2._T1Value ?? (object)Void.MakeVoid());
        if (!t1valueEqual)
            return false;

        bool t2valueEqual = EqualHelper.ValueEqual(obj1._T2Value ?? (object)Void.MakeVoid(), obj2._T2Value ?? (object)Void.MakeVoid());
        if (!t2valueEqual)
            return false;

        return true;
    }

    public static bool operator !=(Union<UnT1, UnT2> obj1, Union<UnT1, UnT2> obj2)
    {
        return !(obj1 == obj2);
    }

    public static bool operator ==(Union<UnT1, UnT2> obj1, UnT1 obj2)
    {
        UnT1 r;
        if (obj1.CheckT1(out r))
            return true;

        return false;
    }

    public static bool operator !=(Union<UnT1, UnT2> obj1, UnT1 obj2)
    {
        return !(obj1 == obj2);
    }

    public static bool operator ==(Union<UnT1, UnT2> obj1, UnT2 obj2)
    {
        UnT2 r;
        if (obj1.CheckT2(out r))
            return true;

        return false;
    }

    public static bool operator !=(Union<UnT1, UnT2> obj1, UnT2 obj2)
    {
        return !(obj1 == obj2);
    }

    public override bool Equals(object? obj)
    {
        Type objType = obj.GetType();
        if (objType == typeof(UnT1) )
        {
            return this == (UnT1)obj;
        }
        else if(objType == typeof(UnT2))
        {
            return this == (UnT2)obj;
        }
        else if(objType == GetType())
        {
            return this == obj.As<Union<UnT1, UnT2>>();
        }

        return false;
    }

    public object Clone()
    {
        Union<UnT1, UnT2> r = null;
        if (_T1Value == null)
            r = new(_T2Value);
        else if (_T2Value == null)
            r = new(_T1Value);
        else
        {
            r = new(_T1Value);
            _ = ~r;
        }
        return r;
    }

    public UnT1? GetValueT1()
    {
        if (_T1Value != null)
        {
            return _T1Value;
        }
        else
        {
            return default;
        }
    }

    public UnT2? GetValueT2()
    {
        if (_T2Value != null)
        {
            return _T2Value;
        }
        else
        {
            return default;
        }
    }

    public T? GetValue<T>()
    {
        if (typeof(T) == typeof(UnT1))
            return (T)Convert.ChangeType(GetValueT1(), typeof(T));
        else if (typeof(T) == typeof(UnT2))
            return (T)Convert.ChangeType(GetValueT1(), typeof(T));
        else
            return default;
    }

    public override int GetHashCode()
    {
        if (_hasValue[0])
            return _T1Value.GetHashCode();
        else if (_hasValue[1])
           return _T2Value.GetHashCode();

        return HashCode.Combine(typeof(UnT1), typeof(UnT2));
    }
}