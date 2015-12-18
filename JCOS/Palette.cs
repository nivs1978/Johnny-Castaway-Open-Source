/*
Palette.cs

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
using System.Drawing;

namespace SCRANTIC
{
  class Palette
  {
    public Color[] color = new Color[16];

    public Palette()
    {
      color[0] = Color.FromArgb(0, 168, 0, 168);
      color[1] = Color.FromArgb(255, 0, 0, 168);
      color[2] = Color.FromArgb(255, 0, 168, 0);
      color[3] = Color.FromArgb(255, 0, 168, 168);
      color[4] = Color.FromArgb(255, 168, 0, 0);
      color[5] = Color.FromArgb(255, 0, 0, 0);
      color[6] = Color.FromArgb(255, 168, 168, 0);
      color[7] = Color.FromArgb(255, 212, 212, 212);
      color[8] = Color.FromArgb(255, 128, 128, 128);
      color[9] = Color.FromArgb(255, 0, 0, 255);
      color[10] = Color.FromArgb(255, 0, 255, 0);
      color[11] = Color.FromArgb(255, 0, 255, 255);
      color[12] = Color.FromArgb(255, 255, 0, 0);
      color[13] = Color.FromArgb(255, 255, 0, 255);
      color[14] = Color.FromArgb(255, 255, 255, 0);
      color[15] = Color.FromArgb(255, 255, 255, 255);
    }
  }
}
