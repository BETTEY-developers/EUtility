using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.Data;

internal class BitDataOperations
{
    public static unsafe byte[] StructToByte(object data)
    {
        int _size = Marshal.SizeOf(data);
        byte[] _bytes = new byte[_size];
        fixed(byte* ptr = &_bytes[0])
        {
            Marshal.StructureToPtr(data, new(ptr), false);
        }
        return _bytes;
    }

    public static byte GetByteHigh4(byte n)
    {
        return (byte)(n >> 4);
    }

    public static byte GetByteLow4(byte n)
    {
        const byte LowMask = 0b00001111;
        return (byte)((n & LowMask) >> 4);
    }

    public static unsafe int GetBitCount<T>(T data)
        where T : struct
    {
        Dictionary<Type, int> _dataBits = new()
        {
            [typeof(byte)] = 8,
            [typeof(short)] = 16,
            [typeof(ushort)] = 16,
            [typeof(int)] = 32,
            [typeof(uint)] = 32,
            [typeof(long)] = 64,
            [typeof(ulong)] = 64,
            [typeof(float)] = 32,
            [typeof(double)] = 64
        };

        if(!(data is byte or short or int or long or ushort or uint or ulong or float or double))
        {
            throw new ArgumentException("Arg 'data' wrong.");
        }
    }
}
