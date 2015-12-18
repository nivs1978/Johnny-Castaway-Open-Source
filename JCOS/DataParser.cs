/*
DataParser.cs

This file is part of Johnny Castaway Open Source.

Copyright (c) 2015 Hans Milling

Johnny Castaway Open Source is free software: you can redistribute it and/or modify it under the terms of the
GNU General Public License as published by the Free Software Foundation, either version 3 of the License,
or (at your option) any later version.
Johnny Castaway Open Source is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU General Public License for more details. You should have received a copy of the
GNU General Public License along with Johnny Castaway Open Source. If not, see http://www.gnu.org/licenses/.

*/
using System;
using System.Collections.Generic;
using System.Text;

namespace SCRANTIC
{
  class DataParser
  {
    private byte[] dta;
    private long offset = 0;

    public DataParser(byte[] data)
    {
      dta = data;
    }

    public void setOffset(UInt32 o)
    {
      offset = o;
    }

    public long getOffset()
    {
      return offset;
    }

    public bool atEnd()
    {
      return offset >= dta.Length;
    }

    public void skip(int i)
  {
    offset += i;
  }

    public byte getByte()
    {
      return dta[offset++];
    }

    public byte peekByte()
    {
      return dta[offset];
    }

    public byte[] getBytes(UInt32 count)
    {
      byte[] bytes = new byte[count];
      for (int i = 0; i < count; i++)
        bytes[i] = getByte();
      return bytes;
    }
    public long bytesLeft()
    {
      return (dta.Length-1) - offset;
    }

    public UInt16 getWord()
    {
      return (UInt16)(getByte() + getByte() * 256);
    }

    public UInt32 getDWord()
    {
      return (UInt32)(getWord() + getWord() * 65536);
    }

    public string getString()
    {
      Byte b = getByte();
      string ret = "";
      while (b != 0)
      {
        ret += Convert.ToChar(b);
        b = getByte();
      }
      return ret;
    }

    public string getStringBlock(int length)
    {
      Byte b = getByte();
      string ret = "";
      while (b != 0)
      {
        ret += Convert.ToChar(b);
        b = getByte();
      }
      for (int i = ret.Length; i < length; i++)
        getByte();
      return ret;
    }

    public string getStringFixed(int length)
    {
      string ret = "";
      bool endreached = false;
      for (int i=0; i<length; i++)
      {
        Byte b = getByte();
        if (b == 0)
          endreached = true;
        if (!endreached)
          ret += Convert.ToChar(b);
      }
      return ret;
    }

  }
}
