using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace hogs_gameManager_wpf
{
    class MapObjectV3
    {
        //source of reference : https://github.com/TalonBraveInfo/OpenHoW/blob/master/src/engine/Map.cpp#L114

        public char[]  name                	{get; set;}    // class name [16]
        public char[]  unused0             	{get; set;}    //[16]
        public short[] position            	{get; set;}    // position in the world [3]
        public ushort  index               	{get; set;}    // todo
        public short[] angles              	{get; set;}    // angles in the world [3]
        public ushort  type               	{get; set;}    // todo
        public short[] bounds              	{get; set;}    // collision bounds [3]
        public ushort  bounds_type         	{get; set;}    // box, prism, sphere and none
        public short   energy			    {get; set;}
        public byte    appearance			{get; set;}
        public byte    team                	{get; set;}    // uk, usa, german, french, japanese, soviet
        public short   objective			{get; set;}
        public byte    objective_actor_id	{get; set;}
        public byte[]  objective_extra     	{get; set;}   //[2]
        public byte    unused1				{get; set;}
        public ushort[] unused2            	{get; set;}   //[8]
        public short[] fallback_position   	{get; set;}   //[3]
        public short   extra			    {get; set;}
        public short   attached_actor_num	{get; set;}
        public short   unused3				{get; set;}

        public MapObjectV3(byte[] hexblock)
        {
            #region arrays
            this.position = new short[3];
            this.angles = new short[3];
            this.bounds = new short[3];
            this.objective_extra = new byte[2];
            this.unused2 = new ushort[8];
            this.fallback_position = new short[3];
            #endregion

            this.name = Encoding.ASCII.GetChars( hexblock[0..16] );
            this.unused0 = Encoding.ASCII.GetChars( hexblock[17..33] );

            //MessageBox.Show(" | " + Convert.ToInt16(hexblock[34] + hexblock[35]) + " | "+BitConverter.ToInt16( hexblock,34 ) + " | " + Convert.ToInt16(Convert.ToInt16(hexblock[34]) + Convert.ToInt16(hexblock[35])) );

            this.position[0] = Convert.ToInt16(hexblock[34] + hexblock[35]);
            this.position[1] = Convert.ToInt16(hexblock[36] +  hexblock[37]);
            this.position[2] = Convert.ToInt16(hexblock[38] +  hexblock[39]);

            this.index = Convert.ToUInt16(hexblock[40] +  hexblock[41]);

            this.angles[0] = Convert.ToInt16(hexblock[42] +  hexblock[43]);
            this.angles[1] = Convert.ToInt16(hexblock[44] +  hexblock[45]);
            this.angles[2] = Convert.ToInt16(hexblock[46] +  hexblock[47]);

            this.type = Convert.ToUInt16(hexblock[48] +  hexblock[49]);

            this.bounds[0] = Convert.ToInt16(hexblock[50] +  hexblock[51]);
            this.bounds[1] = Convert.ToInt16(hexblock[52] +  hexblock[53]);
            this.bounds[2] = Convert.ToInt16(hexblock[53] +  hexblock[54]);

            this.bounds_type = Convert.ToUInt16(hexblock[55] +  hexblock[56]);
            this.energy = Convert.ToInt16(hexblock[57] +  hexblock[58]);
            this.appearance = hexblock[59];
            this.team = hexblock[60];

            this.objective = Convert.ToInt16(hexblock[61] +  hexblock[62]);
            this.objective_actor_id = hexblock[62];
            this.objective_extra[0] = hexblock[63];
            this.objective_extra[1] = hexblock[64];

            this.unused1 = hexblock[65];

            for (int i = 0; i <= 7; i++)    //hexblock[66..81]
            {
                int hexindex = 66 + (i * 2);
                this.unused2[i] = Convert.ToUInt16( hexblock[hexindex] + hexblock[hexindex+1] );
            }


            this.fallback_position[0] = Convert.ToInt16( Convert.ToInt16(hexblock[82] +  hexblock[83]) );
            this.fallback_position[1] = Convert.ToInt16( Convert.ToInt16(hexblock[84] +  hexblock[85]) );
            this.fallback_position[2] = Convert.ToInt16( Convert.ToInt16(hexblock[86] +  hexblock[87]) );

            this.extra = Convert.ToInt16(hexblock[88] +  hexblock[89]);
            this.attached_actor_num = Convert.ToInt16(hexblock[90] +  hexblock[91]);
            this.unused3 = Convert.ToInt16(hexblock[92] +  hexblock[93]);
        }

        public MapObjectV3(byte[] hexblock, bool leboule)
        {
            #region arrays
            this.position = new short[3];
            this.angles = new short[3];
            this.bounds = new short[3];
            this.objective_extra = new byte[2];
            this.unused2 = new ushort[8];
            this.fallback_position = new short[3];
            #endregion

            this.name = Encoding.ASCII.GetChars(hexblock[0..16]);
            this.unused0 = Encoding.ASCII.GetChars(hexblock[17..33]);

            this.position[0] = BitConverter.ToInt16(hexblock, 34);
            this.position[1] = BitConverter.ToInt16(hexblock, 36);
            this.position[2] = BitConverter.ToInt16(hexblock, 38);

            this.index = BitConverter.ToUInt16(hexblock, 40);

            this.angles[0] = BitConverter.ToInt16(hexblock, 42);
            this.angles[1] = BitConverter.ToInt16(hexblock, 44);
            this.angles[2] = BitConverter.ToInt16(hexblock, 46);

            this.type = BitConverter.ToUInt16(hexblock, 48);

            this.bounds[0] = BitConverter.ToInt16(hexblock, 50);
            this.bounds[1] = BitConverter.ToInt16(hexblock, 52);
            this.bounds[2] = BitConverter.ToInt16(hexblock, 54);

            this.bounds_type = BitConverter.ToUInt16(hexblock, 56);
            this.energy = BitConverter.ToInt16(hexblock, 58);
            this.appearance = hexblock[59];
            this.team = hexblock[60];

            this.objective = BitConverter.ToInt16(hexblock, 61);
            this.objective_actor_id = hexblock[62];
            this.objective_extra[0] = hexblock[63];
            this.objective_extra[1] = hexblock[64];

            this.unused1 = hexblock[65];

            for (int i = 0; i <= 7; i++)    //hexblock[66..81]
            {
                int hexindex = 66 + (i * 2);
                this.unused2[i] = BitConverter.ToUInt16(hexblock, hexindex);
            }

            this.fallback_position[0] = BitConverter.ToInt16(hexblock, 82);
            this.fallback_position[1] = BitConverter.ToInt16(hexblock, 84);
            this.fallback_position[2] = BitConverter.ToInt16(hexblock, 86);

            this.extra = BitConverter.ToInt16(hexblock, 88);
            this.attached_actor_num = BitConverter.ToInt16(hexblock, 90);
            this.unused3 = BitConverter.ToInt16(hexblock, 92);
        }

    }
}
