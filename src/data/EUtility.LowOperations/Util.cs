using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.LowOperations;

internal class Util
{
    public static bool IntegerToBoolean(int i) => i switch
    {
        1 => true,
        0 => false
    };

    public static int BooleanToInteger(bool b) => b ? 1 : 0;

    public static bool GetBit<T>(T data, int index)
    {
        return GetBit(GetByte(DataToBytes(data), index), (index + 1) % 4);
    }

    public static unsafe byte[] DataToBytes<T>(T data)
    {
        int _size = Marshal.SizeOf(data);
        byte[] _bytes = new byte[_size];
        fixed (byte* ptr = &_bytes[0])
        {
            Marshal.StructureToPtr(data, new(ptr), false);
        }
        return _bytes;
    }
    public static byte GetByte(byte[] array, int index)
    {
        int _byteindex = ((index + 1) - ((index + 1) % 4)) / 4;
        return array[_byteindex];
    }

    public static bool GetBit(byte bdata, int bitindex)
    {
        return IntegerToBoolean((bdata >> (4 - bitindex)) ^ ((bdata >> (4 - bitindex)) << (4 - bitindex)));
    }
}
