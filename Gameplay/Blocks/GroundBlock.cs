using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class GroundBlock : Block
    {
        public GroundBlock(BlockTemplate template) : base(template)
        {
            BlockType = BlockType.Ground;
        }

        public override void Move(CardinalDirection direction)
        {
            throw new NotImplementedException();
        }
    }
}
