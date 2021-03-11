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

        public override void Move(BlockMovementData movementData)
        {
            Location.Surface = null;
            movementData.Destination.Surface = this;
            this.Location = movementData.Destination;
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
