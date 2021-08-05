using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class CrystalTemplate : CubeTemplate
    {
        public CrystalTemplate(string color) : base(color + "Crystal") { }

        public override SurfaceCube GenerateSurface(Kernel source) => new CrystalCube(this, source);

        public override Cube Generate(Kernel source, CubeMode blockType) => throw new NotImplementedException();
        public override GroundCube GenerateGround(Kernel source) => throw new NotImplementedException();
        public override EphemeralCube GenerateEphemeral(Kernel source) => throw new NotImplementedException();
    }
}
