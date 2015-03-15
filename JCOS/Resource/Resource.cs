using System;
using System.Collections.Generic;
using System.Text;

namespace SCRANTIC.Resource
{
  public abstract class Resource : IDisposable
  {
    public string FileName { get; set;}
    public abstract void parse(byte[] data);
    public Resource(string filename)
    {
      FileName = filename;
    }

    public abstract void Dispose();
  }
}
