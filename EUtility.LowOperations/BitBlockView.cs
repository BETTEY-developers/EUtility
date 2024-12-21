using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.LowOperations;

public class BitBlockView
{
    private bool[] m_bits;

    public BitBlockView(byte[] dataOfBytes)
    {
        m_bits = new bool[dataOfBytes.Length * 8];

        int bindex = 0;
        foreach(byte b in dataOfBytes)
        {
            for(int i = 0; i < 4; i++)
            {
                m_bits[bindex * 4 + i] = Util.GetBit(dataOfBytes[bindex], i);
            }
            bindex += 4;
        }
    }

    public BitBlockView(bool[] bits) => m_bits = bits;

    public BitBlockView(object data) : this(Util.DataToBytes(data))
    {
    }

    public BitBlockViewBitStream GetRawStream() { }
}
