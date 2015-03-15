using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SCRANTIC
{
  public static class Log
  {
    private static object _lock = new object();
    private static string LOGFILE = Path.Combine(Path.GetTempPath(), "JCOS.log");
    public static void write(string text)
    {
      lock (_lock)
      {
        File.AppendAllText(LOGFILE, text + "\r\n");
      }
    }
  }
}
