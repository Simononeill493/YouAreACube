
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract partial class Cube
    {
        public int Energy { get; protected set; }
        public int EnergyCap => Template.MaxEnergy;
        public float EnergyRemainingPercentage => (float)Energy / EnergyCap;

        public bool Zapping;
        public CubeMode ZapTarget;

        public void AddEnergy(int amount)
        {
            if (amount < 1)
            {
                throw new Exception("Can't add less than 1 energy");
            }

            Energy += amount;
            if (Energy > EnergyCap) 
            { 
                Energy = EnergyCap; 
            }

        }
        public virtual void TakeEnergy(int amount)
        {
            if (amount < 1)
            {
                throw new Exception("Can't take less than 1 energy");
            }

            Energy -= amount;
            if (Energy < 0)
            {
                throw new Exception("Took more energy from a block than it has - this shouldn't ever be permitted.");
            }
        }

        public virtual BlockEnergyTransferResult TryTakeEnergyFrom(Cube source, int amount)
        {
            if(amount==0)
            {
                throw new Exception("Tried to take 0 energy from a cube");
            }

            if (amount > source.Energy)
            {
                amount = source.Energy;

                if (amount == 0)
                {
                    return BlockEnergyTransferResult.Failure_NoEnergyInSourceToTake;
                }
            }

            if ((amount+Energy)>EnergyCap)
            {
                var excess = (amount + Energy) - EnergyCap;
                amount -= excess;

                if(amount==0)
                {
                    return BlockEnergyTransferResult.Failure_TargetEnergyIsMaxedOut;
                }
            }

            this.AddEnergy(amount);
            source.TakeEnergy(amount);

            return BlockEnergyTransferResult.Success;
        }
    }

    public enum BlockEnergyTransferResult
    {
        Success,
        Failure_SourceIsDyingEphemeral,
        Failure_NoEnergyInSourceToTake,
        Failure_TargetEnergyIsMaxedOut,
        Failure_CrystalCannotTakeEnergy
    }
}
