using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class EnergyTransferManager
    {
        public void TryGiveEnergy(Cube giver, CardinalDirection cardinalDir,CubeMode cubeMode, int energyAmount)
        {
            if(giver.Location.HasNeighbour(cardinalDir))
            {
                var destination = giver.Location.Adjacent[cardinalDir];
                var giveTarget = destination.GetBlock(cubeMode);

                if (giveTarget != null)
                {
                    giveTarget.TryTakeEnergyFrom(giver, energyAmount);
                }
            }
        }

        public void TrySapEnergy(Cube sapper, CardinalDirection cardinalDir, CubeMode cubeMode)
        {
            if (sapper.Location.HasNeighbour(cardinalDir))
            {
                var destination = sapper.Location.Adjacent[cardinalDir];
                var sapTarget = destination.GetBlock(cubeMode);
                if (sapTarget != null)
                {
                    sapper.TryTakeEnergyFrom(sapTarget, 1);
                }
            }
        }

    }
}
