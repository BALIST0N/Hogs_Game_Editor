using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace hogs_gameEditor_wpf
{
    class MadMtdObject //total : 24 bytes
    {
        public char[] Name     { get; set; }  //[16]
        int DataOffset  { get; set; }  //[4]
        int DataSize    { get; set; }  //[4]
        byte[] ModelData    { get; set; } // model in the .mad starting at this.DataOffset 
        FAC facData { get; set; }

        public MadMtdObject(byte[] hexblock)
        {
            this.Name = Encoding.ASCII.GetChars(hexblock[0..15]);
            this.DataOffset = BitConverter.ToInt32(hexblock, 16);
            this.DataSize = BitConverter.ToInt32(hexblock, 20);
        }
        public MadMtdObject(byte[] hexblock,byte[] modeldata)
        {
            this.Name = Encoding.ASCII.GetChars( hexblock[0..16] );
            this.DataOffset = BitConverter.ToInt32(hexblock, 16);
            this.DataSize = BitConverter.ToInt32(hexblock, 20);

            this.ModelData = modeldata;
        }


        public static List<MadMtdObject> LoadFile(string mapname,string extention)
        {
            List<MadMtdObject> res = new List<MadMtdObject>();
            using (FileStream fs = File.Open("D:/Games/IGG-HogsofWar/Maps/" + mapname + "." + extention, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] mapdata = new byte[fs.Length];
                fs.Read(mapdata, 0, Convert.ToInt32(fs.Length));
                int endContenTable = BitConverter.ToInt32(mapdata, 16); //the first item offset define table content size ! 

                for (int i = 0; i <= endContenTable; i++)
                {
                    int endblock = i + 24;

                    if(endblock < endContenTable)
                    {
                        MadMtdObject tempMod_o = new MadMtdObject(mapdata[i..endblock]);

                        int endDataBlock = tempMod_o.DataOffset + tempMod_o.DataSize;
                        MadMtdObject modobj = new MadMtdObject(mapdata[i..endblock], mapdata[tempMod_o.DataOffset..endDataBlock]);

                        if (new string(modobj.Name).Contains(".FAC") == true )
                        {
                            //modobj.facData = new FAC(modobj.ModelData);
                        }

                        res.Add(modobj);
                    }
                    i += 23;
                }
            }

            return res;
        }

        public static List<MadMtdObject> MergeMadMtd(List<MadMtdObject> baseFile, List<MadMtdObject> ModdedFile)
        {
            int added = 0;
            foreach (MadMtdObject madMtdobj in ModdedFile)
            {
                if (baseFile.Any(x => x.Name.SequenceEqual(madMtdobj.Name)) == false)
                {
                    baseFile.Add(madMtdobj);
                    added++;
                }
            }
            return baseFile;
        }


        public static List<MadMtdObject> recalculateOffsets(List<MadMtdObject> FILE)
        {
            int offset = FILE.Count * 24;        //table content size ! 
            foreach (MadMtdObject madMtdobj in FILE)
            {
                madMtdobj.DataOffset = offset;
                offset += madMtdobj.DataSize;
            }
            return FILE;
        }

        public static void SaveFile(List<MadMtdObject> FILE, string mapName,string extension)
        {
            List<byte> tableContent = new List<byte>();
            List<byte> data = new List<byte>();

            foreach(MadMtdObject madMtdobj in FILE)
            {
                tableContent.AddRange(Encoding.ASCII.GetBytes(madMtdobj.Name));
                tableContent.AddRange(BitConverter.GetBytes(madMtdobj.DataOffset));
                tableContent.AddRange(BitConverter.GetBytes(madMtdobj.DataSize));
                data.AddRange(madMtdobj.ModelData );
            }
            List<byte> res = tableContent;
            res.AddRange(data);

            using (FileStream fs = File.OpenWrite("D:/Games/IGG-HogsofWar/Maps/" + mapName + "_edited." + extension))
            {
                fs.Write(res.ToArray(), 0, res.Count);
            }
 
        }
    }
}
