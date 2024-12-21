using EUtility.Numeric;

namespace EUtility.Value.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Fraction fraction = new(6 / 2);
            Console.WriteLine(fraction.ToString());

            Fraction fraction1 = new(1 / 3);
            Console.WriteLine(fraction1.ToString());
            Console.WriteLine(fraction1.ValueDecimal);

            Fraction fraction2 = fraction + fraction1;
            Console.WriteLine(fraction2.ToString());

            Console.WriteLine((fraction1 *= 3).ToString());
            Console.WriteLine(fraction1.ValueDecimal);

            fraction2 = fraction + fraction1;
            Console.WriteLine(fraction2.ToString());

            fraction2 = ~fraction2;
            Console.WriteLine(fraction2.ToString());
        }
    }
}