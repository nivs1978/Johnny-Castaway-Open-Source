/*
Log.cs

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
using System.IO;
using System.Diagnostics;

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
        Debug.WriteLine(text);
      }
    }
  }
}
