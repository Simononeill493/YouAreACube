﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class DummyTile : Tile
    {
        public DummyTile() : base(IntPoint.Min, IntPoint.Min, IntPoint.Min,0) { }

        public override bool HasThisSurface(SurfaceBlock surface) => true;
        public override bool HasThisGround(GroundBlock ground) => true;
        public override bool HasThisEphemeral(EphemeralBlock ephemeral) => true;
        public override string ToString() => "Dummy Tile";
    }
}
