using System;
using System.Collections.Generic;
using System.Text;

namespace SCRANTIC.Resource
{
  public class TTM : Resource
  {
  /*  public struct ResourceProperty
    {
      public UInt16 Id { get; set; }
      public string Name { get; set; }
    }*/

    public struct TagProperty
    {
      public UInt16 id;
      public string description;
    }
    
    public int SizeVersionString { get; set; }
    public string Version { get; set; }
    public UInt32 Unknown2 { get; set; }
    public UInt16 Unknown3 { get; set; }
    public UInt16 Resources { get; set; }
    //public List<ResourceProperty> resourceproperties = new List<ResourceProperty>();
    public UInt32 TTMSize { get; set; }
    public byte TTMCompressionMethod { get; set; }
    public UInt32 TTMUncompressedSize { get; set; }
    public byte[] TTMData { get; set; }
    public UInt16 Unknown4 { get; set; }
    public UInt16 Unknown5 { get; set; }
    public UInt32 TAGSize { get; set; }
    public UInt16 TagCount { get; set; }
    public bool NeedsInit { get; set; }
    public byte[] script;
    public Dictionary<UInt16, string> tags = new Dictionary<UInt16, string>();
    public Dictionary<int,List<Instruction>> scripts = new Dictionary<int,List<Instruction>>();
    public TTM(string name) : base(name) { }
    private int scene = 0;

    public override void Dispose()
    {
    }

    public override void parse(byte[] data)
    {
      NeedsInit = false;
      DataParser parser = new DataParser(data);
      if (parser.getStringFixed(4) != "VER:")
        throw new Exception("Expected VER: header block");
      SizeVersionString = (int)parser.getDWord();
      Version = parser.getStringFixed(SizeVersionString);
      if (parser.getStringFixed(4) != "PAG:")
        throw new Exception("Expected PAG: header block");
      Unknown2 = parser.getDWord();
      Unknown3 = parser.getWord();
      if (parser.getStringFixed(4) != "TT3:")
        throw new Exception("Expected TT3: header block");
      TTMSize = parser.getDWord() - 5;
      TTMCompressionMethod = parser.getByte();
      TTMUncompressedSize = parser.getDWord();
      TTMData = parser.getBytes(TTMSize);
      if (parser.getStringFixed(4) != "TTI:")
        throw new Exception("Expected TTI: header block");
      Unknown4 = parser.getWord();
      Unknown5 = parser.getWord();
      if (parser.getStringFixed(4) != "TAG:")
        throw new Exception("Expected TAG: header block");
      TAGSize = parser.getDWord();
      TagCount = parser.getWord();
//      System.Diagnostics.Debug.WriteLine("idx\tid\tname");
      for (int j = 0; j < TagCount; j++)
      {
        TagProperty tag = new TagProperty();
        tag.id = parser.getWord();
        tag.description = parser.getString();
        tags.Add(tag.id, tag.description);
//        System.Diagnostics.Debug.WriteLine(j+"\t"+tag.id+"\t"+tag.description);

      }
      Compression comp = new Compression();
      script = comp.decompress(TTMData, TTMCompressionMethod); // Usually RLE/Huffmann
      //string filename = "c:\\temp\\" + this.FileName;
      /*if (System.IO.File.Exists(filename))
        System.IO.File.Delete(filename);
      System.IO.File.WriteAllBytes(filename, script);*/
/*      if (this.FileName == "MEANWHIL.TTM")
      { */
        DataParser moviescript = new DataParser(script);
        //UInt32 unknown6 = moviescript.getDWord();
        try
        {
//          System.Diagnostics.Debug.WriteLine("TTM DUMP START");
          while (!moviescript.atEnd())
          {
            Instruction chunk = new Instruction();
            UInt16 code = moviescript.getWord();
            UInt16 size = (UInt16)(code & 0x000f);
            code &= 0xfff0;
            chunk.code = code;
            if (code == 0x1110 && size == 1)
            {
              UInt16 id = moviescript.getWord();
              chunk.data.Add(id);
              if (tags.ContainsKey(id))
              {
                chunk.name = tags[id];
              }
            }
            else if (size == 15)
            {
              chunk.name = moviescript.getString().ToUpper();
              if (moviescript.peekByte()==0)
                moviescript.skip(1);
              /*if ((moviescript.bytesLeft() & 1) == 1)
              {
                moviescript.skip(1);
              }*/
            }
            /*else if (code == 0x0ff0) // No idea what this is
            {
              for (int i = 0; i < 28; i++)
                chunk.data.Add(moviescript.getByte());
            }*/
            else
            {
              for (int i = 0; i < size; i++)
              {
                chunk.data.Add(moviescript.getWord());
              }
            }
            if (chunk.code == 0x1110)
              scene = chunk.data[0];
            if (!scripts.ContainsKey(scene))
            {
              if (scene == 0)
              {
                tags.Add(0, "init");
                NeedsInit = true;
              }
              scripts.Add(scene, new List<Instruction>());
            }
//            System.Diagnostics.Debug.WriteLine(chunk.ToString());
            scripts[scene].Add(chunk);
          }
//          System.Diagnostics.Debug.WriteLine("TTM DUMP END");

        }
        catch
        {
        }
     // }
    }
  }
}
