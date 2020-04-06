using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Data;

namespace SCRANTIC.Resource
{
    public class ADS : Resource
    {
        public struct ResourceProperty
        {
            public UInt16 id;
            public string name;
        }
        /*
            public struct TagProperty
            {
              public UInt16 id;
              public string description;
            }
        */
        public override void Dispose()
        {
        }

        public int SizeVersionString { get; set; }
        public string Version { get; set; }
        public UInt16 Unknown2 { get; set; }
        public UInt16 Unknown3 { get; set; }
        public UInt32 RESSize { get; set; }
        public UInt16 Resources { get; set; }
        public List<ResourceProperty> resourceproperties = new List<ResourceProperty>();
        public UInt32 SCRSize { get; set; }
        public byte SCRCompressionMethod { get; set; }
        public UInt32 SCRUncompressedSize { get; set; }
        public byte[] SCRData { get; set; }
        public UInt32 TAGSize { get; set; }
        public UInt16 Unknown5 { get; set; }
        public UInt16 TagCount { get; set; }
        public Dictionary<UInt16, string> tags = new Dictionary<UInt16, string>();
        public byte[] script;
        private UInt16 sequence = 0;

        public ADS(string name) : base(name) { }

        public Dictionary<int, List<Instruction>> sequences = new Dictionary<int, List<Instruction>>();

        public override void parse(byte[] data)
        {
            File.WriteAllText(this.FileName, "Decompiled"+"\r\n");

            Debug.WriteLine("ADS Resource");
            DataParser parser = new DataParser(data);
            if (parser.getStringFixed(4) != "VER:")
                throw new Exception("Expected VER: header block");
            SizeVersionString = (int)parser.getDWord();
            Version = parser.getStringFixed(SizeVersionString);
            if (parser.getStringFixed(4) != "ADS:")
                throw new Exception("Expected ADS: header block");
            Unknown2 = parser.getWord();
            Unknown3 = parser.getWord();
            if (parser.getStringFixed(4) != "RES:")
                throw new Exception("Expected RES: header block");
            RESSize = parser.getDWord();
            Resources = parser.getWord();
            Debug.WriteLine("Resource count:" + Resources);
            Debug.WriteLine("Resources:" + Resources);
            File.AppendAllText(this.FileName, "Resources: " + Resources + "\r\n");
            for (int i = 0; i < Resources; i++)
            {
                ResourceProperty resprop = new ResourceProperty();
                resprop.id = parser.getWord();
                resprop.name = parser.getString();
                resourceproperties.Add(resprop);
                Debug.WriteLine(resprop.id.ToString("X2") + " " + resprop.name);
                File.AppendAllText(this.FileName, resprop.id.ToString("X2") + " " + resprop.name + "\r\n");
            }
            if (parser.getStringFixed(4) != "SCR:")
                throw new Exception("Expected SCR: header block");
            SCRSize = parser.getDWord();
            SCRCompressionMethod = parser.getByte();
            SCRUncompressedSize = parser.getDWord();
            SCRData = parser.getBytes(SCRSize - 5); // size+compmethod = -5
            if (parser.getStringFixed(4) != "TAG:")
                throw new Exception("Expected TAG: header block");
            TAGSize = parser.getDWord();
            TagCount = parser.getWord();
            Debug.WriteLine("TAGS:");
            File.AppendAllText(this.FileName, "Tags: " + TagCount + "\r\n");
            for (int j = 0; j < TagCount; j++)
            {
                //TagProperty tag = new TagProperty();
                UInt16 id = parser.getWord();
                string description = parser.getString();
                tags.Add(id, description);
                Debug.WriteLine(id.ToString("X2") + " " + description);
                File.AppendAllText(this.FileName, id.ToString("X2") + " " + description + "\r\n");
            }
            Compression comp = new Compression();
            script = comp.decompress(SCRData, SCRCompressionMethod);

            DataParser dp = new DataParser(script);
            //System.IO.File.WriteAllBytes("c:\\temp\\" + FileName, script);
            List<Instruction> instructions = new List<Instruction>();
            try
            {
                File.AppendAllText(this.FileName, "Script:");
                    Debug.WriteLine("ADS Script:");
                while (!dp.atEnd())
                {
                    Instruction i = new Instruction();
                    UInt16 code = dp.getWord();
                    i.code = code;
                    switch (code)
                    {
                        case 0x1070:
                            i.data.Add(dp.getWord());
                            i.data.Add(dp.getWord());
                            break;
                        case 0x1330:
                            i.data.Add(dp.getWord());
                            i.data.Add(dp.getWord());
                            break;
                        case Instruction.SKIP_NEXT_IF:
                            i.data.Add(dp.getWord());
                            i.data.Add(dp.getWord());
                            break;
                        case Instruction.SKIP_NEXT_IF2:
                            i.data.Add(dp.getWord());
                            i.data.Add(dp.getWord());
                            break;
                        case 0x1370:
                            i.data.Add(dp.getWord());
                            i.data.Add(dp.getWord());
                            break;
                        case 0x1420:
                            break;
                        case Instruction.OR:
                            break;
                        case Instruction.PLAY_SEQUENCES:
                            break;
                        case Instruction.PLAY_SEQUENCES2:
                            break;
                        case 0x1520:
                            i.data.Add(dp.getWord());
                            i.data.Add(dp.getWord());
                            i.data.Add(dp.getWord());
                            i.data.Add(dp.getWord());
                            i.data.Add(dp.getWord());
                            break;
                        case Instruction.ADD_TTM:
                            i.data.Add(dp.getWord());
                            i.data.Add(dp.getWord());
                            i.data.Add(dp.getWord());
                            i.data.Add(dp.getWord());
                            break;
                        case 0x2010:
                            i.data.Add(dp.getWord());
                            i.data.Add(dp.getWord());
                            i.data.Add(dp.getWord());
                            break;
                        case 0x2014:
                            break;
                        case Instruction.RANDOM_START: // Some kind of end
                            break;
                        case Instruction.RANDOM_UNKNOWN1:
                            i.data.Add(dp.getWord());
                            break;
                        case Instruction.RANDOM_END:
                            break;
                        case 0x4000:
                            break;
                        case 0xf010:
                            break;
                        case 0xf200:
                            i.data.Add(dp.getWord());
                            break;
                        case 0xffff: // End of script part perhaps
                            break;
                        default:
                            if (code >= 0x100)
                                throw new Exception("Unsupported opcode");
                            instructions = new List<Instruction>();
                            sequence = code;
                            sequences.Add(code, instructions);
                            break;
                    }
                    if (code >= 0x100)
                        sequences[sequence].Add(i);
                    //System.Diagnostics.Debug.WriteLine(i.ToString());
                    Debug.WriteLine(i.ToString());
                    File.AppendAllText(this.FileName, i.ToString() + "\r\n");                }
            }
            catch
            {
            }

            //System.IO.File.WriteAllBytes(@"c:\temp\" + FileName, script);
        }
        public void dump(string filename)
        {
            List<DataTable> sheets = new List<DataTable>();
            DataTable properties = new DataTable("Properties");
            properties.Columns.Add("id", typeof(string));
            properties.Columns.Add("adsname", typeof(string));
            properties.Columns.Add("ttmname", typeof(string));
            foreach (ResourceProperty rp in resourceproperties)
            {
                DataRow row = properties.NewRow();
                row["id"] = rp.id.ToString("X4");
                row["adsname"] = rp.name;
                row["ttmname"] = "";
                properties.Rows.Add(row);
                TTM ttm = (TTM)ResourceManager.get(rp.name);
                foreach (UInt16 key in ttm.tags.Keys)
                {
                    DataRow r = properties.NewRow();
                    r["id"] = "";
                    r["adsname"] = key.ToString("X4");
                    r["ttmname"] = ttm.tags[key];
                    properties.Rows.Add(r);
                }
            }
            Excel excel = new Excel();
            sheets.Add(properties);

            foreach (UInt16 key in tags.Keys)
            {
                string name = key.ToString("X2") + " " + tags[key];
                DataTable sheet = new DataTable(name);
                sheet.Columns.Add("opcode", typeof(string));
                sheet.Columns.Add("properties", typeof(string));
                sheet.Columns.Add("ttm tag", typeof(string));
                foreach (ResourceProperty rp in resourceproperties)
                {
                    foreach (Instruction i in sequences[key])
                    {
                        DataRow row = sheet.NewRow();
                        row["opcode"] = i.code.ToString("X4");
                        string strprop = "";
                        foreach (UInt16 prop in i.data)
                            strprop += prop.ToString("X4") + " ";
                        row["properties"] = strprop.Trim();
                        // lookup ttm tag name
                        sheet.Rows.Add(row);
                    }
                }
                sheets.Add(sheet);
            }

            File.WriteAllText(filename, excel.DataTablesToExcelXml(sheets, false));
        }
    }
}
