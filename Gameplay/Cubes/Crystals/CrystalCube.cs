using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class CrystalCube : SurfaceCube
    {
        public CrystalCube(CubeTemplate template, Kernel source) : base(template, source) 
        {
            Invincible = true;
            Energy = int.MaxValue;
        }

        public override void TakeEnergy(int amount) { }

        public override BlockEnergyTransferResult TryTakeEnergyFrom(Cube source, int amount)
        {
            return BlockEnergyTransferResult.Failure_CrystalCannotTakeEnergy;
        }
    }
}
