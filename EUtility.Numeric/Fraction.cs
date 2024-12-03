using EUtility.Foundation;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace EUtility.Numeric;

public struct Fraction : INumber<Fraction>
{
    public int Numerator { get; set; }
    public int Denominator { get; set; }

    public readonly decimal ValueDecimal => (decimal)Numerator / (decimal)Denominator;
    public readonly double ValueDouble => (double)Numerator / (double)Denominator;
    public readonly float ValueFloat => (float)Numerator / (float)Denominator;

    public Fraction(double expression, [CallerArgumentExpression("expression")]string expressionString = "")
    {
        (Numerator, Denominator) = ParseToPart(expressionString);
    }

    public Fraction(int numerator, int denominator)
    {
        Numerator = numerator;
        Denominator = denominator;
    }

    private static (int numerator, int denominator) ParseToPart(string expr)
    {
        if (!expr.Contains('/') || expr.Count(x => x == '/') > 1 || string.IsNullOrEmpty(expr))
            throw new ArgumentException("Literal's format is wrong.");

        string[] parts = expr.Split('/');
        if (!int.TryParse(parts[0], out int _) || !int.TryParse(parts[1], out int _))
            throw new ArgumentException("Literal's format is wrong.");

        if (int.Parse(parts[1].Trim(' ')) == 0)
            throw new DivideByZeroException("Denominator cannot be zero.");

        return (int.Parse(parts[0].Trim(' ')), int.Parse(parts[1].Trim(' ')));
    }
    private static bool TryParseInternal(ReadOnlySpan<char> s, out Fraction result)
    {
        try
        {
            Fraction fraction = Parse(s.ToString());
            result = fraction;
            return true;
        }
        catch
        {
            return ParamRef.ReturnFail(out result);
        }
    }

    public static (Fraction left, Fraction right) Each(Fraction left, Fraction right)
    {
        if (left.Denominator != right.Denominator)
        {
            int deno = left.Denominator * right.Denominator;
            Fraction eachedLeft = new(left.Numerator * right.Denominator, deno);
            Fraction eachedRight = new(left.Denominator * right.Numerator, deno);
            return (eachedLeft, eachedRight);
        }
        return (left, right);
    }

    private static bool TryParseInternal(string? s, out Fraction result)
    {
        try
        {
            Fraction fraction = Parse(s);
            result = fraction;
            return true;
        }
        catch
        {
            return ParamRef.ReturnFail(out result);
        }
    }

    private static bool TryConvertFromInternal<TOther>(TOther value, out Fraction result) where TOther : INumberBase<TOther>
    {
        if (value is long)
            return ParamRef.ReturnFail(out result);
        else if (value is ulong)
            return ParamRef.ReturnFail(out result);
        else if (value is Int128)
            return ParamRef.ReturnFail(out result);
        else if (value is UInt128)
            return ParamRef.ReturnFail(out result);

        result = new((int)(object)value, 1);
        return true;
    }

    private static bool TryConvertToInternal<TOther>(Fraction value, out TOther result) where TOther : INumberBase<TOther>
    {
        try
        {
            result = (TOther)(object)value.ValueDecimal;
            return true;
        }
        catch
        {
            return ParamRef.ReturnFail(out result);
        }
    }

    private static bool NullOrOperation(Fraction? left, Fraction? right)
    {
        if (left is null && right is null)
            return true;
        else if (left is null)
            return false;
        else if (right is null)
            return false;

        return true;
    }

    public static Fraction Parse(string expr)
    {
        var parts = ParseToPart(expr);
        return new(parts.numerator, parts.denominator);
    }

    public Fraction Simplified()
    {
        int gcd = Factor.GetGreatestCommonDivisor(Numerator, Denominator);
        return new(Numerator / gcd, Denominator / gcd);
    }

    #region Interface Impl. and Override
    public static Fraction One => new(1 / 1);

    public static int Radix => 10;

    public static Fraction Zero => new(0 / 1);

    public static Fraction AdditiveIdentity => new(1 / 1);

    public static Fraction MultiplicativeIdentity => new(1 / 1);

    public static Fraction Abs(Fraction value) => value;

    public static bool IsCanonical(Fraction value) => true;

    public static bool IsComplexNumber(Fraction value) => false;

    public static bool IsEvenInteger(Fraction value) => (value.Numerator % value.Denominator) == 0 && ((int)value.ValueDecimal % 2) == 0;

    public static bool IsFinite(Fraction value) => true;

    public static bool IsImaginaryNumber(Fraction value) => false;

    public static bool IsInfinity(Fraction value) => false;

    public static bool IsInteger(Fraction value) => (value.Numerator % value.Denominator) == 0;

    public static bool IsNaN(Fraction value) => false;

    public static bool IsNegative(Fraction value) => value.Numerator < 0;

    public static bool IsNegativeInfinity(Fraction value) => false;

    public static bool IsNormal(Fraction value) => true;

    public static bool IsOddInteger(Fraction value) => !IsEvenInteger(value);

    public static bool IsPositive(Fraction value) => value.Numerator > 0;

    public static bool IsPositiveInfinity(Fraction value) => false;

    public static bool IsRealNumber(Fraction value) => true;

    public static bool IsSubnormal(Fraction value) => false;

    public static bool IsZero(Fraction value) => value.Numerator == 0;

    public static Fraction MaxMagnitude(Fraction x, Fraction y)
    {
        if (x.CompareTo(y) > 0)
            return x;
        else
            return y;
    }

    public static Fraction MaxMagnitudeNumber(Fraction x, Fraction y)
    {
        if (x.CompareTo(y) > 0)
            return x;
        else
            return y;
    }

    public static Fraction MinMagnitude(Fraction x, Fraction y)
    {
        if (x.CompareTo(y) < 0)
            return x;
        else
            return y;
    }

    public static Fraction MinMagnitudeNumber(Fraction x, Fraction y)
    {
        if (x.CompareTo(y) < 0)
            return x;
        else
            return y;
    }

    public static Fraction Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => Parse(s.ToString());

    public static Fraction Parse(string s, NumberStyles style, IFormatProvider? provider) => Parse(s);

    public static Fraction Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s.ToString());

    public static Fraction Parse(string s, IFormatProvider? provider) => Parse(s);

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Fraction result) => TryParseInternal(s, out result);

    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Fraction result) => TryParseInternal(s, out result);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Fraction result) => TryParseInternal(s, out result);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Fraction result) => TryParseInternal(s, out result);

    static bool INumberBase<Fraction>.TryConvertFromChecked<TOther>(TOther value, out Fraction result) => TryConvertFromInternal(value, out result);

    static bool INumberBase<Fraction>.TryConvertFromSaturating<TOther>(TOther value, out Fraction result) => TryConvertFromInternal(value, out result);

    static bool INumberBase<Fraction>.TryConvertFromTruncating<TOther>(TOther value, out Fraction result) => TryConvertFromInternal(value, out result);

    static bool INumberBase<Fraction>.TryConvertToChecked<TOther>(Fraction value, out TOther result) => TryConvertToInternal(value, out result);

    static bool INumberBase<Fraction>.TryConvertToSaturating<TOther>(Fraction value, out TOther result) => TryConvertToInternal(value, out result);

    static bool INumberBase<Fraction>.TryConvertToTruncating<TOther>(Fraction value, out TOther result) => TryConvertToInternal(value, out result);

    public int CompareTo(object? obj)
    {
        if (obj is null)
            return -999;

        if (obj is not Fraction)
            return -999;

        Fraction fraction = (Fraction)obj;
        return CompareTo(fraction);
    }

    /// <summary>
    /// Compare fraction.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>If greater than <paramref name="other"/>, return greater than zero; if less than <paramref name="other"/>, return less than zero; if they equals, return zero.</returns>
    public int CompareTo(Fraction? other)
    {
        var eached = Each(this, other.Value);

        if (Numerator > eached.right.Numerator)
            return 1;
        if (Numerator < eached.right.Numerator)
            return -1;
        if (Numerator > eached.right.Numerator)
            return 0;

        return int.MinValue;
    }

    public bool Equals(Fraction? other)
    {
        if (!other.HasValue)
            return false;

        return ValueDecimal == other.Value.ValueDecimal;
    }

    public string ToString(string? format, IFormatProvider? formatProvider) => format.Replace("$N$", Numerator.ToString()).Replace("$D$", Denominator.ToString());

    public override string ToString()
    {
        return Numerator + "/" + Denominator;
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        destination = new Span<char>(ToString(format.ToString(), provider).ToCharArray());
        charsWritten = destination.Length;
        return true;
    }

    public int CompareTo(Fraction other)
    {
        var eached = Each(this, other);

        if (Numerator > eached.right.Numerator)
            return 1;
        if (Numerator < eached.right.Numerator)
            return -1;
        if (Numerator > eached.right.Numerator)
            return 0;

        return int.MinValue;
    }

    public bool Equals(Fraction other) => ValueDecimal == other.ValueDecimal;
    public override bool Equals(object obj) => obj is Fraction && Equals((Fraction)obj);

    #endregion

    #region Operators
    public static Fraction operator +(Fraction value) => value;

    public static Fraction operator +(Fraction left, Fraction right)
    {
        var eached = Each(left, right);
        return new(eached.left.Numerator + eached.right.Numerator, eached.left.Denominator);
    }

    public static Fraction operator -(Fraction value) => new(-value.Numerator, value.Denominator);

    public static Fraction operator -(Fraction left, Fraction right)
    {
        var eached = Each(left, right);
        return new(eached.left.Numerator - eached.right.Numerator, eached.left.Denominator);
    }

    public static Fraction operator ++(Fraction value) => new(value.Numerator + value.Denominator, value.Denominator);

    public static Fraction operator --(Fraction value) => new(value.Numerator - value.Denominator, value.Denominator);

    public static Fraction operator *(Fraction left, Fraction right) => new(left.Numerator * right.Numerator, left.Denominator * right.Denominator);

    public static Fraction operator /(Fraction left, Fraction right) => new(left.Numerator * right.Denominator, left.Denominator * right.Numerator);

    public static Fraction operator %(Fraction left, Fraction right)
    {
        var eached = Each(left, right);
        return new(eached.left.Numerator % right.Numerator, eached.left.Denominator);
    }

    public static bool operator ==(Fraction? left, Fraction? right)
    {
        if (!NullOrOperation(left, right))
            return false;

        return left.Value.Equals(right);
    }

    public static bool operator !=(Fraction? left, Fraction? right) => !(left == right);

    public static bool operator <(Fraction left, Fraction right) => left.CompareTo(right) < 0;

    public static bool operator >(Fraction left, Fraction right) => left.CompareTo(right) > 0;

    public static bool operator <=(Fraction left, Fraction right) => left.CompareTo(right) <= 0;

    public static bool operator >=(Fraction left, Fraction right) => left.CompareTo(right) >= 0;

    public static bool operator ==(Fraction left, Fraction right) => left.CompareTo(right) == 0;

    public static bool operator !=(Fraction left, Fraction right) => left.CompareTo(right) != 0;

    public static Fraction operator ~(Fraction value) => value.Simplified();

    public static explicit operator int(Fraction value) => (int)value.ValueFloat;
    public static explicit operator uint(Fraction value) => (uint)value.ValueFloat;
    public static explicit operator long(Fraction value) => (long)value.ValueFloat;
    public static explicit operator ulong(Fraction value) => (ulong)value.ValueFloat;

    public static implicit operator double(Fraction value) => value.ValueDouble;
    public static implicit operator float(Fraction value) => value.ValueFloat;
    public static implicit operator decimal(Fraction value) => value.ValueDecimal;

    public static implicit operator Fraction(int value) => new(value, 1);
    #endregion
}
