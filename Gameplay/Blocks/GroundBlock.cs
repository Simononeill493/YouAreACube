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
        public GroundBlock(BlockTemplate template) : base(template, BlockMode.Ground) { }

        public override void EnterLocation(Tile destination)
        {
            if (destination.HasThisGround(this))
            {
                throw new Exception("Tried to add a ground to a location it already exists in");
            }
            if (!Location.IsDummy)
            {
                throw new NotImplementedException("Ground already has a position - moving ground from place to place is not implemented");
            }

            Location.Ground = null;
            this.Location = destination;
            Location.Ground = this;
        }

        public override bool CanOccupyDestination(Tile destination) => throw new NotImplementedException();
        public override bool ToBeDeleted() => throw new NotImplementedException();
    }
}
