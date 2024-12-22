using EUtility.ConvertEx.Convert;

namespace EUtility.StringEx.StringExtension;

public static class StringExtension
{
    /// <summary>
    /// Get string in console width.
    /// </summary>
    /// <param name="str">would get string</param>
    /// <returns><paramref name="str"/> in console width</returns>
    public static int GetStringInConsoleGridWidth(this string str)
        => NullLib.ConsoleEx.ConsoleText.CalcStringLength(str);

    /// <summary>
    /// Get Converter
    /// </summary>
    /// <param name="str">would get converter string</param>
    /// <returns>A <see cref="Converter{T}"/> value;</returns>
    public static Converter<string> GetConverter(this string str) => new(str);
}