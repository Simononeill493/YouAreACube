using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public partial class SurfaceBlock : Block
    {
        public bool Dormant = false;

        public SurfaceBlock(BlockTemplate template): base(template)
        {
            BlockType = BlockMode.Surface;
            Health = MaxHealth;
        }

        public override void EnterLocation(Tile destination)
        {
            if (destination.HasThisSurface(this))
            {
                throw new Exception("Tried to add a ground to a location it already exists in");
            }

            if (!Location.HasThisSurface(this))
            {
                throw new Exception("Surface block is being moved, but its current tile does not register it as present.");
            }

            Location.Surface = null;
            this.Location = destination;
            Location.Surface = this;
        }

        public void GoDormant()
        {
            Dormant = true;
            ColorMask = (128, 128, 128, 255);
        }

        public override bool CanOccupyDestination(Tile destination) => !destination.HasSurface;
        public override bool ToBeDeleted() => false;
        public override bool CanUpdate => !Dormant;
    }
}
