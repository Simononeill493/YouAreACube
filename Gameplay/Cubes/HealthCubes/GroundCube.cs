using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class GroundCube : HealthCube
    {
        public GroundCube(CubeTemplate template) : base(template, CubeMode.Ground) { }

        public override void EnterLocation(Tile destination)
        {
            if (destination.HasThisGround(this))
            {
                throw new Exception("Tried to add a ground to a location it already exists in");
            }
            if (!Location.HasThisGround(this))
            {
                throw new Exception("Ground block is being moved, but its current tile does not register it as present.");
            }

            Location.Ground = null;
            this.Location = destination;
            Location.Ground = this;
        }

        public override bool CanOccupyDestination(Tile destination) => !destination.HasGround;
    }
}
