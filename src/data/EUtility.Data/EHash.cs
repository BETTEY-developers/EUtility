using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace EUtility.Data;

public struct EHash
{
    private string _hash;
    private char[] _hashTable = "!@#$%^&*()_+1234567890-=qwertyuiop[]\\{}|asdfghjkl;':\"zxcvbnm,./<>?*".ToArray();

    internal EHash(string hash)
    {
        _hash = hash;
    }

    public static EHash GetEHash(object data)
    {
        byte[] dataBytes = BitDataOperations.StructToByte(data);
        int master = 0;
        byte mask = 0b10101101;
        
        foreach(var b in dataBytes)
        {
            byte l = BitDataOperations.GetByteLow4(b);
            byte h = BitDataOperations.GetByteHigh4(b);
            int a1 = l + h;
            int a2 = l * h;
            int a3 = l / h;
            int a4 = h / l;
            int a5 = l - h;
            int a6 = h - l;
            int a7 = h % l;
            int a8 = l % h;

            int am = (a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8) / a1;

        }
    }
}
