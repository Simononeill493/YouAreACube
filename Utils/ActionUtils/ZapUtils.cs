using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ZapUtils
    {
        public static void TryZapOutOfTurn(Cube actor)
        {
            if(actor!=null && actor.Zapping)
            {
                TryZap(actor, actor.ZapTarget);
            }
        }

        public static void TryZap(Cube actor, CubeMode blockType)
        {
            actor.Zapping = true;
            actor.ZapTarget = blockType;

            if (actor.Location.HasCube(blockType) & !actor.ToBeDeleted() & actor.Energy > 0)
            {
                var energy = actor.Energy;
                actor.TakeEnergy(energy);
                actor.Location.GetBlock(blockType).DealDamage(energy);

                /*if (actor.Location.AbsoluteLocation.DistanceFrom(Kernel.HostLoc) < 30)
                {
                    SoundInterface.PlayPop();
                }*/
            }
        }

    }
}
