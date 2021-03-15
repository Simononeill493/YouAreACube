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
                Console.WriteLine("Location does not match surface?");
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
