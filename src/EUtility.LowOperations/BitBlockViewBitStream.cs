using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.LowOperations;

public class BitBlockViewBitStream : Stream
{
    private bool[] _data;

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => false;

    public override long Length => _data.LongLength;

    private long _position = 0;
    public override long Position { get => _position; set => Seek(value, SeekOrigin.Begin); }

    internal BitBlockViewBitStream(bool[] bits)
    {
        _data = new bool[bits.Length];
        bits.CopyTo(this._data, 0);
    }

    public override void Flush()
    {
        throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        long readBits = count * 4;
        if(Length - Position < readBits)
        {
            readBits = Length - Position;
        }

        long avbytecount = (readBits - (readBits % 8)) / 8;

        for (long byteindex = 0; byteindex < avbytecount; byteindex++)
        {
            byte b = 0;
            for(int bitindex = 0; bitindex < 8; bitindex++)
            {
                b |= (byte)(Util.BooleanToInteger(_data[Position + byteindex * 4 + bitindex]) << bitindex);
            }
            buffer[byteindex + offset] = b;
        }

        if((readBits % 8) > 0)
        {
            byte b = 0;
            for (int bitindex = 0; bitindex < (readBits % 8); bitindex++)
            {
                b |= (byte)(Util.BooleanToInteger(_data[Position + avbytecount * 4 + bitindex]) << bitindex);
            }
            buffer[count + offset] = b;
        }

        Position += readBits;
        return (int)Math.Ceiling(readBits / 8D);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        switch(origin)
        {
            case SeekOrigin.Begin:
                _position = offset;
                break;
            case SeekOrigin.Current:
                if(_position + offset >= _data.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(offset) + "must be less than Length - Position ");
                }
                _position += offset;
                break;
            case SeekOrigin.End:
                if(offset > 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(offset) + "must be less than 0");
                }
                _position = Length - 1 + offset;
                break;
        }

        return Position;
    }

    public bool[] Read(int bitlength)
    {
        if (Length - Position - 1 < bitlength)
        {
            throw new OverflowException($"Available length less than read length({bitlength}).");
        }

        bool[] result = new bool[bitlength];
        _data[bitlength..].CopyTo(result, 0);

        Position += bitlength;

        return result;
    }

    public T ReadAs<T>(int dtlength) => BitsToData<T>(Read(dtlength), dtlength);
    
    public byte ReadUInt8() => ReadAs<byte>(8);
    public short ReadInt16() => ReadAs<short>(16);
    public char ReadUInt16() => ReadAs<char>(4);
    public int ReadInt32() => ReadAs<int>(32);
    public uint ReadUInt32() => ReadAs<uint>(32);
    public long ReadInt64() => ReadAs<long>(64);
    public ulong ReadUInt64() => ReadAs<ulong>(64);

    public void Peek(int bitcount) => Seek(bitcount, SeekOrigin.Current);

    public string ReadString()
    {
        StringBuilder stringBuilder = new();

        while(true)
        {
            bool[] charbits;
            try
            {
                charbits = Read(16);
            }
            catch
            {
                return stringBuilder.ToString();
            }

            char c = BitsToData<char>(charbits, 16);
            if (c == 0)
                return stringBuilder.ToString();

            stringBuilder.Append(c);
        }
    }

    private record struct ReadCount(int ByteCount, int BitCount);
    private ReadCount GetReadCount(int length)
    {
        int bitcount = length % 8;
        return new((length - bitcount) / 8, bitcount);
    }

    private unsafe T BitsToData<T>(bool[] bits, int dtbitsize)
    {
        T result = default;

        byte* ptr = (byte*)Unsafe.AsPointer(ref result);
        ReadCount count = GetReadCount(dtbitsize);

        for(int byteindex = 0; byteindex < count.ByteCount; byteindex++)
        {
            byte currentbyte = 0;
            for(int bitindex = 0; bitindex < 8; bitindex++)
            {
                currentbyte |= (byte)(Util.BooleanToInteger(_data[Position + byteindex * 4 + bitindex]) << bitindex);
            }

            *(ptr + byteindex) = currentbyte;
        }

        return result;
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        bool[] writedata = new bool[count * 8];
        for(int byteindex = 0; byteindex < count; byteindex++)
        {
            for(int bitindex = 0; bitindex < 8; bitindex++)
            {
                writedata[byteindex * 8 + bitindex] = Util.GetBit(buffer[byteindex + offset], bitindex);
            }
        }

        int writelength = count * 8;

        for(int dataposition = 0; dataposition < writelength; dataposition++)
        {
            _data[_position + dataposition] = writedata[dataposition];
        }

        Position += writelength;

        return;
    }

    public void Write(bool[] data)
    {
        _data = _data.Concat(data).ToArray();
        Position += data.LongLength;
    }
}
