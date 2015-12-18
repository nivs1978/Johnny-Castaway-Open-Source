/*
INI.cs

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

namespace SCRANTIC
{
  class INI
  {
    List<string> lines = null;
    string inifile = null;

    public INI()
    {
    }

    public void load(string file)
    {
      inifile = file;
      if (System.IO.File.Exists(file))
        lines = new List<string>(System.IO.File.ReadAllLines(file));
      else
      {
        lines = new List<string>();
      }
    }

    public void save()
    {
      string dir = inifile.Substring(0, inifile.LastIndexOf('\\'));
      Directory.CreateDirectory(dir);
      File.WriteAllLines(inifile, lines.ToArray(), Encoding.ASCII);
    }

    public void set(string key, string value)
    {
      bool found = false;
      int idx = 0;
      foreach (string line in lines)
      {
        int i = line.IndexOf('=');
        if (i >= 0)
        {
          string k = line.Substring(0, i);
          if (key.ToLower() == k.ToLower())
          {
            found = true;
            lines[idx] = key + "=" + value;
            break;
          }
        }
        idx++;
      }
      if (!found) // Add this at bottom of ini file
      {
        lines.Add(key + "=" + value);
      }
    }

    public string get(string key)
    {
      foreach (string line in lines)
      {
        if (line.IndexOf('=') >= 0)
        {
          int idx = line.IndexOf('=');
          string k = line.Substring(0, idx);
          if (key.ToLower() == k.ToLower())
          {
            if (idx == line.Length - 1)
              return "";
            else
              return line.Substring(idx + 1);
          }
        }
      }
      return null; // Not found
    }

  }
}
