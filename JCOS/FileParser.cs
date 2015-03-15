using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SCRANTIC
{

  class FileParser
  {
//    private byte[] data;
    private DataParser parser;

    public FileParser(string filename)
    {
      if (File.Exists(filename))
        parser = new DataParser(System.IO.File.ReadAllBytes(filename));
      else
      {
        MessageBox.Show("File not found: " + filename, "Johnny Castaway Open Source");
        throw new Exception("Unable to read file: " + filename);
      }
    }

    public void setOffset(UInt32 o)
    {
      parser.setOffset(o);
    }

    public byte getByte()
    {
      return parser.getByte();
    }

    public byte[] getBytes(UInt32 count)
    {
      return parser.getBytes(count);
    }

    public UInt16 getWord()
    {
      return parser.getWord();
    }

    public UInt32 getDWord()
    {
      return parser.getDWord();
    }

    public string getString()
    {
      return parser.getString();
    }

    public string getStringBlock(int length)
    {
      return parser.getStringBlock(length);
    }

  }
}
