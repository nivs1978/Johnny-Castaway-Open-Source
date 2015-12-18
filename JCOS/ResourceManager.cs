/*
ResourceManager.cs

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

namespace SCRANTIC
{
  static class ResourceManager
  {
    public static Map map = null;
    private static Dictionary<string, Resource.Resource> resources = new Dictionary<string, Resource.Resource>(StringComparer.CurrentCultureIgnoreCase);
    private static string resourcemap = "C:\\SIERRA\\SCRANTIC\\RESOURCE.MAP";

    public static Resource.Resource get(string name)
    {
      if (resources.ContainsKey(name.ToLower()))
      {
        return resources[name];
      }
      else
      {
        if (map == null)
        {
          map = new Map();

          map.parse(resourcemap); 
        }
        FileParser reader = new FileParser(SCRANTIC.ResourceManager.map.ResourceFile);
        int index = SCRANTIC.ResourceManager.map.getResourceIndex(name);
        if (index < 0)
          return null;
        UInt32 offset = SCRANTIC.ResourceManager.map.getOffset(index);
        reader.setOffset(offset);
        string filename = reader.getStringBlock(12);
        UInt32 size = reader.getDWord();
        byte[] data = reader.getBytes(size);
        string ext = name.Substring(name.LastIndexOf(".") + 1).ToLower();
        switch (ext)
        {
          case "ads":
            Resource.ADS adsres = new Resource.ADS(name);
            adsres.parse(data);
            resources.Add(name.ToLower(), adsres);
            return adsres;
          case "bmp":
            Resource.BMP bmpres = new Resource.BMP(name);
            bmpres.parse(data);
            resources.Add(name.ToLower(), bmpres);
            return bmpres;
          case "pal":
            Resource.PAL palres = new Resource.PAL(name);
            palres.parse(data);
            resources.Add(name.ToLower(), palres);
            return palres;
          case "scr":
            Resource.SCR scrres = new Resource.SCR(name);
            scrres.parse(data);
            resources.Add(name.ToLower(), scrres);
            return scrres;
          case "ttm":
            Resource.TTM ttmres = new Resource.TTM(name);
            ttmres.parse(data);
            resources.Add(name.ToLower(), ttmres);
            return ttmres;
          case "vin":
            break;
        }
      }
     return null;
    }
  }
}
