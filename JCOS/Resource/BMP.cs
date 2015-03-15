using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Drawing;

namespace SCRANTIC.Resource
{
  /// <summary>
  /// BMP class represents a set of bitmap images (16 color/4bit). 
  /// </summary>
  class BMP : Resource
  {
    public UInt16 Width { get; set;}
    public UInt16 Height { get; set;}
    public UInt32 DataSize { get; set;}
    public UInt16 Images { get; set;}
    public UInt16[] Widths { get; set;}
    public UInt16[] Heights { get; set;}
    public UInt32 BMPSize { get; set;}
    public byte BMPCompressionMethod { get; set;}
    public UInt32 BMPUncompressedSize { get; set;}
    public byte[] BMPData { get; set;}
    public UInt32 VGASize { get; set;}
    public byte VGACompressionMethod { get; set;}
    public UInt32 VGAUncompressedSize { get; set;}
    public byte[] VGAData { get; set;}
    public List<Bitmap> images = new List<Bitmap>();

    public BMP(string name) : base(name){}

    public override void parse(byte[] data)
    {
      DataParser parser = new DataParser(data);
      if (parser.getStringFixed(4) != "BMP:")
        throw new Exception("Expected BMP: header block");
      Width = parser.getWord();
      Height = parser.getWord();
      if (parser.getStringFixed(4) != "INF:")
        throw new Exception("Expected INF: header block");
      DataSize = parser.getDWord(); // size of "no of images" + (images*(size of with+height)) 
      Images = parser.getWord();
      Widths = new UInt16[Images];
      for (int j = 0; j < Images; j++)
        Widths[j] = parser.getWord();
      Heights = new UInt16[Images];
      for (int j = 0; j < Images; j++)
        Heights[j] = parser.getWord();
      if (parser.getStringFixed(4) != "BIN:")
        throw new Exception("Expected BIN: header block");
      BMPSize = parser.getDWord() - 5; // discard size of compressionmethod+uncompressedsize
      BMPCompressionMethod = parser.getByte();
      BMPUncompressedSize = parser.getDWord();
      BMPData = parser.getBytes(BMPSize);

      Compression lzw = new Compression();
      data = lzw.decompress(BMPData, BMPCompressionMethod);
      images = Tools.getBitmaps(data, Widths, Heights);
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append(FileName);
      sb.Append("\t");
      sb.Append(Width);
      sb.Append("\t");
      sb.Append(Height);
      sb.Append("\t");
      sb.Append(Images);
      sb.Append("\t");
      sb.Append(BMPCompressionMethod);
      sb.Append("\t");
      sb.Append(BMPSize - 5);
      sb.Append("\t");
      sb.Append(BMPUncompressedSize);
      sb.Append("\t");
      sb.Append("1:");
      double comp = BMPUncompressedSize;
      comp = comp / BMPSize;
      sb.AppendFormat(comp.ToString("0.00",CultureInfo.InvariantCulture));
      sb.Append("\t");
      sb.Append("");
      for (int i = 0; i < Images; i++)
      {
        sb.Append(Widths[i]);
        sb.Append("x");
        sb.Append(Heights[i]);
        if (i < Images - 1)
          sb.Append(", ");
      }
      sb.AppendLine("");
      return sb.ToString();
    }

    public override void Dispose()
    {
      if (images!=null)
      {
        for (int i = 0; i< images.Count; i++)
        {
          images[i].Dispose();
          images[i] = null;
        }
      }
    }

  }
}
