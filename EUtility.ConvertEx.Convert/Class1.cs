namespace EUtility.ConvertEx.Convert
{
    public class Converter<T> where T : IConvertible
    {
        T Value { get; set; }
        public Converter(T value)
        {
            Value = value;
        }

        short ToInt16(IFormatProvider? formatProvider) => Value.ToInt16(formatProvider);
        int ToInt32(IFormatProvider? formatProvider) => Value.ToInt32(formatProvider);
        long ToInt64(IFormatProvider? formatProvider) => Value.ToInt64(formatProvider);
        ushort ToUInt16(IFormatProvider? formatProvider) => Value.ToUInt16(formatProvider);


    }
}