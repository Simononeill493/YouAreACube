using System;

namespace IAmACube
{
    [Serializable()]
    public class ZapManager
    {
        public void TryZap(Block actor, BlockMode blockType)
        {
            if (actor.Location.HasBlock(blockType) & !actor.ToBeDeleted())
            {
                var energy = actor.Energy;
                actor.TakeEnergy(energy);
                actor.Location.GetBlock(blockType).DealDamage(energy);
            }
        }
    }
}