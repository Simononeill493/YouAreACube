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
        public GroundBlock(BlockTemplate template) : base(template) => BlockType = BlockMode.Ground;
        public override bool CanOccupyDestination(Tile destination) => throw new NotImplementedException();
        public override bool ShouldBeDestroyed() => throw new NotImplementedException();


        public override void EnterLocation(Tile destination)
        {
            if (destination.HasThisGround(this))
            {
                throw new Exception("Tried to add a ground to a location it already exists in");
            }

            Location.Ground = null;
            destination.Ground = this;
            this.Location = destination;
        }
    }
}
