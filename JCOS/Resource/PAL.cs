using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SCRANTIC.Resource
{
  class PAL : Resource
  {
    public Color[] Color;

    public PAL(string name) : base(name) { }

    public override void parse(byte[] data)
    {
      DataParser parser = new DataParser(data);
      if (parser.getStringFixed(4) != "PAL:")
        throw new Exception("Expected PAL: header block");
      UInt16 size = parser.getWord();
      byte unknown1 = parser.getByte();
      byte unknown2 = parser.getByte();
      if (parser.getStringFixed(4) != "VGA:")
        throw new Exception("Expected VGA: header block");
      Color = new Color[256];
      for (int i = 0; i < 256; i++)
      {
        Color c = System.Drawing.Color.FromArgb(255, parser.getByte()*4, parser.getByte()*4, parser.getByte()*4);
        Color[i] = c;
      }
    }

    public override void Dispose()
    {
      Color = null;
    }
  }
}
