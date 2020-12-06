/*
FileParser.cs

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
using System.Windows.Forms;

namespace SCRANTIC
{

    class FileParser
    {
        //    private byte[] data;
        private DataParser parser;

        public FileParser(string filename)
        {
            if (File.Exists(filename))
            {
                parser = new DataParser(System.IO.File.ReadAllBytes(filename));
            }
            else
            {
                MessageBox.Show("File not found: " + filename, "Johnny Castaway Open Source");
                throw new Exception("Unable to read file: " + filename);
            }
        }

        public void setOffset(UInt32 o)
        {
            parser.setOffset(o);
        }

        public byte getByte()
        {
            return parser.getByte();
        }

        public byte[] getBytes(UInt32 count)
        {
            return parser.getBytes(count);
        }

        public UInt16 getWord()
        {
            return parser.getWord();
        }

        public UInt32 getDWord()
        {
            return parser.getDWord();
        }

        public string getString()
        {
            return parser.getString();
        }

        public string getStringBlock(int length)
        {
            return parser.getStringBlock(length);
        }

    }
}
