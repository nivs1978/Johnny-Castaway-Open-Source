using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SCRANTIC
{
  class Compression
  {
    int nextbit;
    int current;
    int offset;
    byte[] data;

    private byte readByte()
    {
      if (offset >= data.Length)
        return 0;
      else
      {
        byte b = data[offset++];
        return b;
      }
    }

    private void skip(int n)
    {
      offset += n;
    }

    private void skipBits()
    {
      if (nextbit > 0)
      {
        skip(1);
        nextbit = 0;
        current = readByte();
      }
    }

    private uint getBits(uint n)
    {
      if (n == 0)
        return 0;
      uint x = 0;
      for (uint i = 0; i < n; i++)
      {
        if ((current & (1 << nextbit)) != 0)
          x += (uint)(1 << (int)i);
        nextbit++;
        if (nextbit > 7)
        {
          current = readByte();
          nextbit = 0;
        }
      }
      return x;
    }

    struct CodeTableEntry
    {
      public ushort prefix;
      public byte append;
    }

    public byte[] decompress(byte[] dta, byte compressionmethod)
    {
      data = dta;
      switch (compressionmethod)
      {
        case 0:
          return data;
        case 1:
          return decompressRLE();
        case 2:
          return decompressLZW();
        case 3:
          return decompressRLE2();
      }
      return null;
    }

    private byte[] decompressLZW()
    {
      List<byte> pdata = new List<byte>();
      current = readByte();
      //int posout = 0;
      nextbit = 0;
      CodeTableEntry[] codetable = new CodeTableEntry[4096];
      byte[] decodestack = new byte[4096];
      int stackptr = 0;
      uint n_bits = 9;
      uint free_entry = 257;
      uint oldcode = getBits(n_bits);
      uint lastbyte = oldcode;
      uint bitpos = 0;
      //tw.WriteLine("pdata["+pdata.Count+"] = " + oldcode);
      pdata.Add((byte)oldcode);
      try
      {
        while (offset < data.Length)
        {
          uint newcode = getBits(n_bits);
          bitpos += n_bits;
          if (newcode == 256)
          {
            uint nbits3 = n_bits << 3;
            uint nskip = (nbits3 - ((bitpos - 1) % nbits3)) - 1;
            uint dummy = getBits(nskip);
            n_bits = 9;
            free_entry = 256;
            bitpos = 0;
          }
          else
          {
            uint code = newcode;
            if (code >= free_entry)
            {
              if (stackptr >= 4096)
                break;
              decodestack[stackptr] = (byte)lastbyte;
              stackptr++;
              code = oldcode;
            }
            while (code >= 256)
            {
              if (code > 4095)
                break;
              decodestack[stackptr] = codetable[code].append;
              stackptr++;
              code = codetable[code].prefix;
            }
            decodestack[stackptr] = (byte)code;
            stackptr++;
            lastbyte = code;
            while (stackptr > 0)
            {
              stackptr--;
              //tw.WriteLine("pdata[" + pdata.Count + "] = " + decodestack[stackptr]);
              pdata.Add(decodestack[stackptr]);
            }
            if (free_entry < 4096)
            {
              codetable[free_entry].prefix = (ushort)oldcode;
              codetable[free_entry].append = (byte)lastbyte;
              free_entry++;
              int temp = 1 << (int)n_bits;
              if (free_entry >= temp && n_bits < 12)
              {
                n_bits++;
                bitpos = 0;
              }
            }
            oldcode = newcode;
          }
        }
      }
      catch { } // Throw away exception and return data
      return pdata.ToArray();
    }

    public byte[] decompressRLE()
    {
      offset = 0;
      List<byte> pdata = new List<byte>();
      while (offset < data.Length)
      {
        byte control = readByte();
        if ((control & 0x80) == 0x80)
        {
          byte length = (byte)(control & 0x7F);
          byte b = readByte();
          for (int i = 0; i < length; i++)
            pdata.Add(b);
        }
        else
        {
          for (int i = 0; i < control; i++)
            pdata.Add(readByte());
        }
      }
      return pdata.ToArray();
    }

    public byte[] decompressRLE2()
    {
      List<byte> pdata = new List<byte>();
      while (offset < data.Length)
      {
        byte control = readByte();
        if ((control & 0x80) == 0x80)
        {
          byte length = readByte();
          for (int i = 0; i < length; i++)
            pdata.Add((byte)(control & 0x7F));
        }
        else
        {
          for (int i = 0; i < control; i++)
            pdata.Add(readByte());
        }
      }
      return pdata.ToArray();
    }

  }
}
