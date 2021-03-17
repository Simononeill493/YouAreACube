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
            BlockType = BlockType.Surface;
        }

        public override void MoveToCurrentDestination()
        {
            if(Location.Surface!=this)
            {
                throw new Exception("Surface block is being moved, but its current tile does not register it as present.");
            }

            Location.Surface = null;
            MovementData.Destination.Surface = this;
            this.Location = MovementData.Destination;
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
