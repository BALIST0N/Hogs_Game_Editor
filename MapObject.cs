using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xceed.Wpf.Toolkit;

namespace hogs_gameManager_wpf
{
    class MapObject
    {

        //sources of reference :
        //https://github.com/DummkopfOfHachtenduden/how-doc/blob/master/Map/POG%20-%20PigObjectGround.cs
        // & https://github.com/TalonBraveInfo/OpenHoW/blob/master/doc/file-formats/POG.md

        
        public byte[] Name { get; set; }        //Model name
        public byte[] NULL;             //"NULL"
        public ushort XOffset { get; set; }
        public ushort YOffset { get; set; }
        public ushort ZOffset { get; set; }

        public ushort ID { get; set; }
        public ushort XRotation { get; set; }         // 4096 = 360°
        public ushort YRotation { get; set; }        // 4096 = 360°
        public ushort ZRotation { get; set; }          // 4096 = 360°
        public ushort TypeID { get; set; }          // Model type id (instancing)

        public ushort BoundingBoxWidth { get; set; }
        public ushort BoundingBoxHeight { get; set; }
        public ushort BoundingBoxLength { get; set; }

        // BoundingBoxType:
        // - Box = 0
        // - Prism = 1
        public ushort BoundingBoxType { get; set; }

        public ushort SpawnDelay { get; set; }
        public byte unkByte0 { get; set; }            //Bitflag destruct-/damageabiltiy?

        // PigTeam:
        // - Team01 = 0x01
        // - Team02 = 0x02
        // - Team03 = 0x04
        // - Team04 = 0x08
        // - Team05 = 0x10
        // - Team06 = 0x20
        // - Team07 = 0x40
        // - Team08 = 0x80
        public byte Team { get; set; }

        // ScriptType:
        // - DESTROY_ITEM           = 01
        // - DESTROY_PROPOINT       = 02
        // - PROTECT_ITEM           = 03
        // - PROTECT_PROPOINT       = 04
        // - DROPZONE_ITEM          = 07
        // - DROPZONE_PROPOINT      = 08
        // - DESTROY_GROUP_ITEM     = 13
        // - DESTROY_GROUP_PROPOINT = 14
        // - PICKUP_ITEM            = 19
        // - TUTORIAL_DESTROY       = 20
        // - TUTORIAL_END           = 21
        // - TUTORIAL_BLAST         = 22
        // - TUTORIAL_DESTROY_GROUP = 23
        public ushort ScriptType { get; set; }

        public byte ScriptGroup { get; set; }

        public byte weaponID { get; set; }
        public byte amount { get; set; }
        public byte[] advanced { get; set; }

        public ushort ScriptXOffset { get; set; }
        public ushort ScriptYOffset { get; set; }
        public ushort ScriptZOffset { get; set; }

        // ObjectFlag:
        // - None      = 0
        // - Player    = 1 << 0,
        // - Bit1      = 1 << 1,
        // - Bit2      = 1 << 2,
        // - Bit3      = 1 << 3,
        // - ScriptObj = 1 << 4,
        // - Inside    = 1 << 5
        // - Delayed   = 1 << 6
        // - Bit7      = 1 << 7
        public ushort Flag { get; set; }

        public ushort PigSpawnDelay { get; set; }
        public ushort unkUShort2 { get; set; }

        public MapObject(byte[] hexblock)
        {
            this.Name = hexblock[0..16];     
            this.NULL = hexblock[17..33];

            this.XOffset = BitConverter.ToUInt16( hexblock,34 );
            this.YOffset = BitConverter.ToUInt16(hexblock,36);
            this.ZOffset = BitConverter.ToUInt16(hexblock,38);

            this.ID = BitConverter.ToUInt16(hexblock,40);
            this.XRotation = BitConverter.ToUInt16(hexblock,42);
            this.YRotation = BitConverter.ToUInt16(hexblock,44);
            this.ZRotation = BitConverter.ToUInt16(hexblock,46);
            this.TypeID = BitConverter.ToUInt16(hexblock,48);

            this.BoundingBoxWidth = BitConverter.ToUInt16(hexblock,50);
            this.BoundingBoxHeight = BitConverter.ToUInt16(hexblock,52);
            this.BoundingBoxLength = BitConverter.ToUInt16(hexblock,52);

            this.BoundingBoxType = BitConverter.ToUInt16(hexblock,56);

            this.SpawnDelay = BitConverter.ToUInt16(hexblock,58);
            this.unkByte0 = hexblock[59];

            this.Team = hexblock[60];

            this.ScriptType = BitConverter.ToUInt16(hexblock,61);
            this.ScriptGroup = hexblock[63];
            this.weaponID = hexblock[64];
            this.amount = hexblock[65];
            this.advanced = hexblock[66..81];
            this.ScriptXOffset = BitConverter.ToUInt16(hexblock,82);                
            this.ScriptYOffset = BitConverter.ToUInt16(hexblock,84);
            this.ScriptZOffset = BitConverter.ToUInt16(hexblock,86);

            this.Flag = BitConverter.ToUInt16(hexblock,88);
            this.PigSpawnDelay = BitConverter.ToUInt16(hexblock,90);
            this.unkUShort2 = BitConverter.ToUInt16(hexblock,92);

        }
    }
}
