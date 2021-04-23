using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class EphemeralBlock : Block
    {
        private bool _ephemeralFaded;

        public EphemeralBlock(BlockTemplate template) : base(template)
        {
            BlockType = BlockMode.Ephemeral;
        }

        public override void Update(UserInput input, ActionsList actions)
        {
            base.Update(input, actions);

            if(Energy>0)
            {
                TakeEnergy(1);
            }
        }

        public override void TakeEnergy(int amount)
        {
            base.TakeEnergy(amount);

            if(Energy==0)
            {
                FadeAway();
            }
        }

        public void FadeAway()
        {
            if(!Location.HasThisEphemeral(this))
            {
                throw new Exception("Ephemeral faded away but it's not in the right location");
            }

            Location.Ephemeral = null;
            _ephemeralFaded = true;
        }

        public override void EnterLocation(Tile destination)
        {
            if(destination.HasThisEphemeral(this))
            {
                throw new Exception("Tried to add an ephemeral to a location it already exists in");
            }

            if (destination.HasEphemeral)
            {
                var absorbResult = TryAbsorbInto(destination.Ephemeral);
                if(absorbResult == BlockEnergyTransferResult.Success)
                {
                    //Console.WriteLine("Ephemeral " + _id + " at " + Location.AbsoluteLocation + " absorbed into Ephemeral " + destination.Ephemeral._id + " at " + destination.Ephemeral.Location.AbsoluteLocation);
                    return;
                }
            }

            Location.Ephemeral = null;
            this.Location = destination;
            Location.Ephemeral = this;
        }


        private BlockEnergyTransferResult TryAbsorbInto(EphemeralBlock destination) => destination.TryTakeEnergyFrom(this,Energy);

        public override BlockEnergyTransferResult TryTakeEnergyFrom(Block source, int amount)
        {
            if(source.BlockType == BlockMode.Ephemeral)
            {
                if(source.ShouldBeDestroyed())
                {
                    return BlockEnergyTransferResult.Failure_SourceIsDyingEphemeral;
                }
            }

            return base.TryTakeEnergyFrom(source, amount);
        }


        public override bool CanOccupyDestination(Tile destination)
        {
            return true;
        }

        public override bool ShouldBeDestroyed() => _ephemeralFaded;

        public override void BeCreatedBy(Block creator)
        {
            base.BeCreatedBy(creator);
            creator.TakeEnergy(Template.InitialEnergy);
        }
    }
}
