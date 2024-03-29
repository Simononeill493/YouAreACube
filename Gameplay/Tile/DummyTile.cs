﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class DummyTile : Tile
    {
        public DummyTile() : base(IntPoint.MinValue, IntPoint.MinValue, IntPoint.MinValue,IntPoint.Zero) { }

        public override bool HasThisSurface(SurfaceCube surface) => true;
        public override bool HasThisGround(GroundCube ground) => true;
        public override bool HasThisEphemeral(EphemeralCube ephemeral) => true;
        public override string ToString() => "Dummy Tile";
    }
}
