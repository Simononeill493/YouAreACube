using System;
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
            BlockType = BlockMode.Surface;
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
            destination.Surface = this;
            this.Location = destination;
        }


        public override bool CanOccupyDestination(Tile destination)
        {
            return !destination.HasSurface;
        }

        public override bool ShouldBeDestroyed()
        {
            return false;
        }
    }
}
