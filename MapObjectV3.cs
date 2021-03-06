﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace hogs_gameManager_wpf
{
    [CategoryOrder("General", 1)]
    [CategoryOrder("Script", 2)]
    [CategoryOrder("unused", 3)]
    public class MapObjectV3
    {
        //source of reference : https://github.com/TalonBraveInfo/OpenHoW/blob/master/src/engine/Map.cpp#L114

        public char[]  name                	{get; set;}    // class name [16]
        public char[]  unused0             	{get; set;}    //[16]
        public short[] position            	{get; set;}    // position in the world (0 = x, 1 = z, 2 = y) [3]
        public ushort  index               	{get; set;}    // Id of the object on the map 
        public short[] angles              	{get; set;}    // angles in the world [3]
        public ushort  type               	{get; set;}    // todo | looks like its the skin/model number or the rank for a pig
        public short[] bounds              	{get; set;}    // collision bounds [3]
        public ushort  bounds_type         	{get; set;}    // box, prism, sphere and none
        public short   energy			    {get; set;}    //sometimes its the number of turn before the item drop... if its 255 it means its default health
        public byte    appearance			{get; set;}    //??? appear it with a parachute at start of the map ???
        public byte    team                	{get; set;}    // uk, usa, german, french, japanese, soviet
        public ushort  objective			{get; set;}
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

            this.name = Encoding.ASCII.GetChars(hexblock[0..16]);
            this.unused0 = Encoding.ASCII.GetChars(hexblock[16..32]);

            this.position[0] = BitConverter.ToInt16(hexblock, 32);
            this.position[1] = BitConverter.ToInt16(hexblock, 34);
            this.position[2] = BitConverter.ToInt16(hexblock, 36);

            this.index = BitConverter.ToUInt16(hexblock, 38);

            this.angles[0] = BitConverter.ToInt16(hexblock, 40);
            this.angles[1] = BitConverter.ToInt16(hexblock, 42);
            this.angles[2] = BitConverter.ToInt16(hexblock, 44);

            this.type = BitConverter.ToUInt16(hexblock, 46);

            this.bounds[0] = BitConverter.ToInt16(hexblock, 48);
            this.bounds[1] = BitConverter.ToInt16(hexblock, 50);
            this.bounds[2] = BitConverter.ToInt16(hexblock, 52);

            this.bounds_type = BitConverter.ToUInt16(hexblock, 54);
            this.energy = BitConverter.ToInt16(hexblock, 56);
            this.appearance = hexblock[58];
            this.team = hexblock[59];

            this.objective = BitConverter.ToUInt16(hexblock, 60);
            this.objective_actor_id = hexblock[62];
            this.objective_extra[0] = hexblock[63];
            this.objective_extra[1] = hexblock[64];

            this.unused1 = hexblock[65];

            for (int i = 0; i <= 7; i++)    //hexblock[65..81]
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

        public MapObjectV3()
        {

        }

        public byte[] ConvertToByteArray()
        {
            List<byte> hexList = new List<byte>();
            byte[] hexblock = new byte[94];

            hexList.AddRange( Encoding.ASCII.GetBytes(this.name) );
            hexList.AddRange(Encoding.ASCII.GetBytes(this.unused0) );

            hexList.AddRange(BitConverter.GetBytes(this.position[0]));
            hexList.AddRange(BitConverter.GetBytes(this.position[1]));
            hexList.AddRange(BitConverter.GetBytes(this.position[2]));

            hexList.AddRange( BitConverter.GetBytes(this.index) );

            hexList.AddRange(BitConverter.GetBytes(this.angles[0]));
            hexList.AddRange(BitConverter.GetBytes(this.angles[1]));
            hexList.AddRange(BitConverter.GetBytes(this.angles[2]));

            hexList.AddRange(BitConverter.GetBytes(this.type));


            hexList.AddRange(BitConverter.GetBytes(this.bounds[0]));
            hexList.AddRange(BitConverter.GetBytes(this.bounds[1]));
            hexList.AddRange(BitConverter.GetBytes(this.bounds[2]));

            hexList.AddRange(BitConverter.GetBytes(this.bounds_type));
            hexList.AddRange(BitConverter.GetBytes(this.energy));
            hexList.Add( this.appearance );
            hexList.Add( this.team );

            hexList.AddRange(BitConverter.GetBytes(this.objective));

            hexList.Add(this.objective_actor_id);
            hexList.Add(this.objective_extra[0]);
            hexList.Add(this.objective_extra[1]);
            hexList.Add(this.unused1);

            for (int i = 0; i <= 7; i++)    //hexblock[65..81]
            {
                hexList.AddRange( BitConverter.GetBytes(this.unused2[i]) );
            }

            hexList.AddRange(BitConverter.GetBytes(this.fallback_position[0]));
            hexList.AddRange(BitConverter.GetBytes(this.fallback_position[1]));
            hexList.AddRange(BitConverter.GetBytes(this.fallback_position[2]));

            hexList.AddRange(BitConverter.GetBytes(this.extra));
            hexList.AddRange(BitConverter.GetBytes(this.attached_actor_num));
            hexList.AddRange(BitConverter.GetBytes(this.unused3));

            hexblock = hexList.ToArray(); //do it like this for testing if it crash
            return hexblock;
        }

    }
}
