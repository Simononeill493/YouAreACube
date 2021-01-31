﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class SurfaceBlock : Block
    {        
        public SurfaceBlock(BlockTemplate template): base(template)
        {
            BlockType = BlockType.Surface;
        }

        protected override void _move(Tile destination)
        {
            if (CanOccupyDestination(destination))
            {
                Location.Surface = null;
                destination.Surface = this;
                this.Location = destination;
            }
        }

        public override bool CanOccupyDestination(Tile destination)
        {
            return !destination.HasSurface;
        }
    }
}
