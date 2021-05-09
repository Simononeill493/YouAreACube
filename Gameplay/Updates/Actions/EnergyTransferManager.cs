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
        public void TryGiveEnergy(Block actor, CardinalDirection cardinalDir,BlockMode blockMode, int energyAmount)
        {
            var destination = actor.Location.Adjacent[cardinalDir];
            var target = destination.GetBlock(blockMode);

            if (target != null)
            {
                target.TryTakeEnergyFrom(actor,energyAmount);
            }
        }
    }
}
