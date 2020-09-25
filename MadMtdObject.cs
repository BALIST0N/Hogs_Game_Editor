using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace hogs_gameEditor_wpf
{
    class MadMtdObject //total : 24 bytes (both file types have same structure)
    {
        public char[] Name     { get; set; }  //[16]
        int DataOffset  { get; set; }  //[4]
        int DataSize    { get; set; }  //[4]
        byte[] ModelData    { get; set; } // model in the .mad starting at this.DataOffset
        FAC facData { get; set; }

        public MadMtdObject(byte[] hexblock)
        {
            this.Name = Encoding.ASCII.GetChars(hexblock[0..16]);
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
                    int endblockContentTable = i + 24;

                    if(endblockContentTable <= endContenTable)
                    {
                        MadMtdObject modobj = new MadMtdObject(mapdata[i..endblockContentTable]);

                        int endDataBlock = modobj.DataOffset + modobj.DataSize;
                        modobj.ModelData = mapdata[modobj.DataOffset..endDataBlock];

                        if (new string(modobj.Name).Contains(".FAC") == true )
                        {
                            modobj.facData = new FAC(modobj.ModelData);
                        }

                        res.Add(modobj);
                    }
                    i += 23;
                }               
            }
            
            /*
            string text = mapname +"."+ extention + "\n";
            foreach(MadMtdObject mmobj in res)
            {
                text += new string(mmobj.Name) + "=" + mmobj.DataOffset + "="+ mmobj.DataSize  + "\n";
                string filename = new string(mmobj.Name).TrimEnd('\0');
                File.WriteAllBytes("D:/Games/IGG-HogsofWar/devtools/ext_ed/" + filename, mmobj.ModelData);
            }
            File.WriteAllText("D:/Games/IGG-HogsofWar/devtools/ext_ed/fakeLog.ini", text);*/

            return res;
        }


        public static List<MadMtdObject> MergeWithModdedMadMtd(List<MadMtdObject> baseFile, List<MadMtdObject> ModdedFile)
        {
            foreach (MadMtdObject madMtdobj in ModdedFile)
            {
                if (baseFile.Any(x => x.Name.SequenceEqual(madMtdobj.Name)) == false) {  baseFile.Add(madMtdobj); } //check if model or texture already exist in the basefile
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

        public static List<MadMtdObject> ModifyFACIndexes(List<MadMtdObject> MAD, List<MadMtdObject> MTD)
        {
            foreach(MadMtdObject madobj in MAD)
            {
                if(madobj.facData != null)
                {
                    string facName = new string(madobj.Name).Trim('\0');
                    facName = facName.Substring(0,facName.Length - 4);
                    int counter = 0;
                    bool skipTriangles = false;

                    foreach(MadMtdObject mtdobj in MTD)
                    {
                        if(new string(mtdobj.Name).Contains(facName) == true )
                        {
                            if(madobj.facData.triangleCount != 0 && skipTriangles == false)
                            {
                                if(counter < madobj.facData.triangleCount)
                                {
                                    madobj.facData.triangleTextureIndex[counter] = MTD.IndexOf(mtdobj);
                                }
                                else
                                {
                                    skipTriangles = true;
                                    counter = 0;
                                }
                            }
                            else
                            {
                                madobj.facData.planeTextureIndex[counter] = MTD.IndexOf(mtdobj);
                            }
                            counter++;
                        }
                    }
                    madobj.ModelData = FAC.OverrideHexIndexes(madobj.facData, madobj.ModelData);
                }
            }
            return MAD;
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
                data.AddRange(madMtdobj.ModelData);
            }
            List<byte> res = tableContent;
            res.AddRange(data);

            if(extension == "mtd")
            {
                mapName = mapName.ToLower();
            }

            using (FileStream fs = File.OpenWrite("D:/Games/IGG-HogsofWar/Maps/" + mapName + "_edited." + extension))
            {
                fs.Write(res.ToArray(), 0, res.Count);
            }
            MessageBox.Show("Saved file " + mapName + "_edited." + extension);
 
        }
    }
}
