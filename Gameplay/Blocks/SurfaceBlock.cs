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

        public override void Move(CardinalDirection direction)
        {
            var destination = Location.Adjacent[direction];
            if (!destination.HasSurface)
            {
                Location.Contents = null;
                destination.Contents = this;
                this.Location = destination;
            }
        }
    }
}
