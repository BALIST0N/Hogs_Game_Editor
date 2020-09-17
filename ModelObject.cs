using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace hogs_gameEditor_wpf
{
    class ModelObject //total : 24 bytes
    {
        public char[] Name     { get; set; }  //[16]
        int DataOffset  { get; set; }  //[4]
        int DataSize    { get; set; }  //[4]

        byte[] ModelData    { get; set; } // model in the .mad starting at this.DataOffset 



        public ModelObject(byte[] hexblock)
        {
            this.Name = Encoding.ASCII.GetChars(hexblock[0..15]);
            this.DataOffset = BitConverter.ToInt32(hexblock, 16);
            this.DataSize = BitConverter.ToInt32(hexblock, 20);
        }
        public ModelObject(byte[] hexblock,byte[] modeldata)
        {
            this.Name = Encoding.ASCII.GetChars( hexblock[0..16] );
            this.DataOffset = BitConverter.ToInt32(hexblock, 16);
            this.DataSize = BitConverter.ToInt32(hexblock, 20);

            this.ModelData = modeldata;
        }


        public static List<ModelObject> LoadMADFile(string mapname)
        {
            List<ModelObject> res = new List<ModelObject>();
            using (FileStream fs = File.Open("D:/Games/IGG-HogsofWar/Maps/" + mapname + ".MAD", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] mapdata = new byte[fs.Length];
                fs.Read(mapdata, 0, Convert.ToInt32(fs.Length));
                int endContenTable = BitConverter.ToInt32(mapdata, 16); //the first item offset define table content size ! 

                for (int i = 0; i <= endContenTable; i++)
                {
                    int endblock = i + 24;

                    if(endblock < endContenTable)
                    {
                        ModelObject tempMod_o = new ModelObject(mapdata[i..endblock]);

                        int endDataBlock = tempMod_o.DataOffset + tempMod_o.DataSize;
                        ModelObject modobj = new ModelObject( mapdata[i..endblock], mapdata[tempMod_o.DataOffset..endDataBlock] );

                        res.Add(modobj);
                    }
                    i += 23;
                }
            }

            return res;
        }

        public static List<ModelObject> recalculateOffsets(List<ModelObject> MADFILE)
        {
            int offset = MADFILE.Count * 24;        //table content size ! 
            foreach (ModelObject mod in MADFILE)
            {
                mod.DataOffset = offset;
                offset += mod.DataSize;
            }
            return MADFILE;
        }

        public static void SaveMadFile(List<ModelObject> MADFILE, string mapName)
        {
            List<byte> tableContent = new List<byte>();
            List<byte> data = new List<byte>();

            foreach(ModelObject mod in MADFILE)
            {
                tableContent.AddRange(Encoding.ASCII.GetBytes(mod.Name));
                tableContent.AddRange(BitConverter.GetBytes(mod.DataOffset));
                tableContent.AddRange(BitConverter.GetBytes(mod.DataSize));
                data.AddRange( mod.ModelData );
            }
            List<byte> res = tableContent;
            res.AddRange(data);
             
            using (FileStream fs = File.OpenWrite("D:/Games/IGG-HogsofWar/Maps/" + mapName + "_edited.MAD"))
            {
                fs.Write(res.ToArray(), 0, res.Count);
            }
 
        }
    }
}
