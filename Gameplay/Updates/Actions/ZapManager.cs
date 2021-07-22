using System;

namespace IAmACube
{
    [Serializable()]
    public class ZapManager
    {
        public void TryZap(Cube actor, CubeMode blockType)
        {
            if (actor.Location.HasBlock(blockType) & !actor.ToBeDeleted())
            {
                var energy = actor.Energy;
                actor.TakeEnergy(energy);
                actor.Location.GetBlock(blockType).DealDamage(energy);


                if(actor.Location.AbsoluteLocation.DistanceFrom(Kernel.HostLoc) < 30)
                {
                    SoundInterface.PlayPop();
                }
            }
        }
    }
}