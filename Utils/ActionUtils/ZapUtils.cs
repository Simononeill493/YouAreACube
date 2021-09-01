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
                var target = actor.Location.GetBlock(blockType);

                var energy = actor.Energy;

                actor.TakeEnergy(energy);
                target.DealDamage(energy);

                /*if (actor.Location.AbsoluteLocation.DistanceFrom(Kernel.HostLoc) < 30)
                {
                    SoundInterface.PlayPop();
                }*/
            }
        }

    }
}
