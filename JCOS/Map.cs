using System;
using System.Collections.Generic;
using System.Text;

namespace SCRANTIC
{
  public class Map
  {
    private class Entry
    {
      private UInt32 _bytes;
      private UInt32 _offset;
      private string _name;

      public Entry(UInt32 bytes, UInt32 offset, string name)
      {
        _bytes = bytes;
        _offset = offset;
        _name = name;
      }

      public UInt32 Bytes { get { return _bytes; } set { _bytes = value; } }
      public UInt32 Offset { get { return _offset; } set { _offset = value; } }
      public string Name { get { return _name; } set { _name = value; } }
    }

    private byte[] header = { 0, 0, 0, 0, 0, 0 };
    private string resourcefile = "";
    private UInt16 resources = 0;
    private List<Entry> entries = new List<Entry>();

    public byte[] Header { get { return header; } set { header = value; } }
    public string ResourceFile { get { return resourcefile; } set { resourcefile = value; } }
    public UInt16 Resources { get { return resources; } set { resources = value; } }

    public int getResourceIndex(string name)
    {
      int count = 0;
      foreach (Entry e in entries)
      {
        if (e.Name.ToLower() == name.ToLower())
          return count;
        count++;
      }
      return -1;
    }

    public void addEntry(UInt32 bytes, UInt32 offset, string name)
    {
      entries.Add(new Entry(bytes, offset, name));
    }

    public UInt32 getOffset(int no)
    {
      return entries[no].Offset;
    }

    public void parse(string mapfile)
    {
      FileParser reader = new FileParser(mapfile);
      for (int i = 0; i < 6; i++)
      {
        Header[i] = reader.getByte();
      }
      string path = Tools.filePath(mapfile)+"\\";
      ResourceFile = path+reader.getStringBlock(12);
      FileParser resreader = new FileParser(ResourceFile);
      Resources = reader.getWord();
      for (int i = 0; i < Resources; i++)
      {
        UInt32 bytes = reader.getDWord();
        UInt32 offset = reader.getDWord();
        resreader.setOffset(offset);
        string name = resreader.getStringBlock(12);
        addEntry(bytes, offset, name);
      }
    }

  }
}
