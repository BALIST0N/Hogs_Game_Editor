using System;
using System.Collections.Generic;
using System.Text;

namespace hogs_gameEditor_wpf
{
    class FAC
    {
        char[] reserved { get; set; }             // reserved for name?
        int triangleCount { get; set; }  
        public List<int> triangleTextureIndex { get; set; }   // index from .MTD
        int planeCount { get; set; }
        public List<int> planeTextureIndex { get; set; }
        


        public FAC(byte[] hexblock)
        {
            
            triangleTextureIndex = new List<int>();
            planeTextureIndex = new List<int>();

            this.reserved = Encoding.ASCII.GetChars( hexblock[0..16] );
            this.triangleCount = BitConverter.ToInt32(hexblock, 16);

            int index = 20;     //(16 + 4)
            if (this.triangleCount != 0 )
            {
                //to pick the first "triangle.TextureIndex " in the byte array : index + 20)
                for (int i = 0; i < this.triangleCount; i++)
                {
                    index += 20;
                    triangleTextureIndex.Add(BitConverter.ToInt32(hexblock, index));
                    index += 12;
                }
            }

            this.planeCount = BitConverter.ToInt32(hexblock, index);

            if(this.planeCount != 0 )
            {
                index += 4; //plane.count int skip
                for (int i = 0; i < this.planeCount; i++)
                {
                    index += 24;
                    planeTextureIndex.Add(BitConverter.ToInt32(hexblock, index));
                    index += 12;
                }
            }

        }
    }
}