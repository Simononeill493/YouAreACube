﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class BlockTemplate
    {
        public string Name;
        public string Sprite;
        public bool Active;
        public int Speed;

        public ChipBlock Chips;

        public BlockTemplate(string name)
        {
            Name = name;
        }

        public SurfaceBlock GenerateSurface()
        {
            var block = new SurfaceBlock(this);
            //todo set any dynamic data here
            return block;
        }
        public GroundBlock GenerateGround()
        {
            var block = new GroundBlock(this);
            //todo set any dynamic data here
            return block;
        }
    }
}