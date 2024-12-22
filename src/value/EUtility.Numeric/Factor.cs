using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.Numeric;

public static class Factor
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<int> GetFactors(int num)
    {
        HashSet<int> result = new HashSet<int>()
        {
            1,
            num
        };

        for(int factor = 2; factor < num; factor++)
        {
            if((num % factor) == 0)
            {
                (bool, bool) addresult = (result.Add(factor), result.Add(num / factor));
                if(!addresult.Item1 || !addresult.Item2)
                {
                    break;
                }
            }
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<int> GetCommonFactors(int left, int right)
    {
        HashSet<int> leftFactors = GetFactors(left);
        HashSet<int> rightFactors = GetFactors(right);

        return leftFactors.Intersect(rightFactors);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetGreatestCommonDivisor(int left, int right)
    {
        return GetCommonFactors(left, right).Max();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetLeastCommonMultiple(int left, int right)
    {
        List<int> cfs = GetCommonFactors(left, right).ToList();
        int result = 0;
        foreach (int item in cfs)
        {
            result *= item;
        }

        return result;
    }
}
