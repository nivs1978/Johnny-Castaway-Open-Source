using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SCRANTIC.Resource
{
  class SCR : Resource
  {
    public UInt16 TotalSize { get; set; }
    public UInt16 Flags { get; set; }
    public UInt32 DIMSize { get; set; }
    public UInt16 Width { get; set; }
    public UInt16 Height { get; set; }
    public UInt32 BMPDataSize { get; set; }
    public byte BMPCompressionMethod { get; set; }
    public UInt32 BMPUncompressedSize { get; set; }
    public byte[] BMPData { get; set; }
    public UInt32 VGASize { get; set; }
    public byte VGACompressionMethod { get; set; }
    public UInt32 VGAUncompressedSize { get; set; }
    public byte[] VGAData { get; set; }
    public Bitmap image;

    public override void Dispose()
    {
    }

    public SCR(string name) : base(name) { }

    public override void parse(byte[] data)
    {
      DataParser parser = new DataParser(data);
      if (parser.getStringFixed(4) != "SCR:")
        throw new Exception("Expected SCR: header block");
      TotalSize = parser.getWord();
      Flags = parser.getWord();
      if (parser.getStringFixed(4) != "DIM:")
        throw new Exception("Expected DIM: header block");
      DIMSize = parser.getDWord(); // size of "no of images" + (images*(size of with+height)) 
      Width = parser.getWord();
      Height = parser.getWord();
      if (parser.getStringFixed(4) != "BIN:")
        throw new Exception("Expected BIN: header block");
      BMPDataSize = parser.getDWord() - 5; // discard size of compressionmethod+uncompressedsize
      BMPCompressionMethod = parser.getByte();
      BMPUncompressedSize = parser.getDWord();
      BMPData = parser.getBytes(BMPDataSize);

      Compression lzw = new Compression();
      data = lzw.decompress(BMPData, BMPCompressionMethod);
      List<Bitmap> images = Tools.getBitmaps(data, new UInt16[ 1 ] { Width }, new UInt16[1] { Height });
      image = images[0];
    }

  }
}
