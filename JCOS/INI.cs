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
