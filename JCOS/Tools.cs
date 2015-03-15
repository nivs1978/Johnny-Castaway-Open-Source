using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SCRANTIC
{
  class Tools
  {
    /// <summary>
    /// Get the path to a file
    /// </summary>
    /// <param name="filename">full file path</param>
    /// <returns>Directory path</returns>
    public static string filePath(string filename)
    {
      int idx = filename.LastIndexOf("\\");
      if (idx >= 0)
        return filename.Substring(0, idx);
      else return null;
    }

    public static List<Bitmap> getBitmaps(byte[] data, UInt16[] widths, UInt16[] heights)
    {
      List<Bitmap> ret = new List<Bitmap>();;
      Palette pal = new Palette();
      int idx = 0;
      int bitidx = 0;
      for (int i = 0; i < widths.Length; i++)
      {
        Bitmap bmp = new Bitmap((int)widths[i], (int)heights[i]);
        Graphics g = Graphics.FromImage(bmp);
        try
        {
          for (int y = 0; y < heights[i]; y++)
          {
            for (int x = 0; x < widths[i]; x++)
            {
              int color = data[idx];
              if (bitidx % 2 == 0)
                color = color >> 4;
              else
              {
                color = color & 0x0f;
                idx++;
              }
              Color c = pal.color[color];
              Brush b = new SolidBrush(c);
              g.FillRectangle(b, x, y, 1, 1);
              b.Dispose();
              bitidx++;
            }
          }
        } catch {}
        g.Save();
        g.Dispose();
        ret.Add(bmp);
      }
      return ret;
    }

    public static string getHex(byte[] data)
    {
      string hex = BitConverter.ToString(data).Replace("-", "");
      string ret = "";
      while (hex.Length > 32)
      {
        string str = hex.Substring(0, 32);
        string s = "";
        while (str.Length>0)
        {
          s = s + str.Substring(0, 2)+" ";
          str = str.Substring(2);
          }
        ret += s + "\r\n";
        hex = hex.Substring(32);
      }
      if (hex.Length > 32)
        ret += hex;
      return ret;
    }

  }
}
