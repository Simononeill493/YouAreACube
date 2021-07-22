using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class DummyTile : Tile
    {
        public DummyTile() : base(IntPoint.Min, IntPoint.Min, IntPoint.Min,0) { }

        public override bool HasThisSurface(SurfaceCube surface) => true;
        public override bool HasThisGround(GroundCube ground) => true;
        public override bool HasThisEphemeral(EphemeralCube ephemeral) => true;
        public override string ToString() => "Dummy Tile";
    }
}
