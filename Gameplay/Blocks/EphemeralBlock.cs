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


        public override void EnterLocation(Tile destination)
        {
            if(destination.HasThisEphemeral(this))
            {
                throw new Exception("Tried to add an ephemeral to a location it already exists in");
            }

            if (destination.HasEphemeral)
            {
                AbsorbInto(destination.Ephemeral);
                return;
            }

            Location.Ephemeral = null;
            destination.Ephemeral = this;
            this.Location = destination;
        }


        private void AbsorbInto(EphemeralBlock destination)
        {
            destination.AddEnergy(this.Energy);
            this.TakeEnergy(this.Energy);
        }

        public override bool CanOccupyDestination(Tile destination)
        {
            return true;
        }

        public override bool ShouldBeDestroyed()
        {
            return (Energy < 1);
        }

        public override void BeCreatedBy(Block creator)
        {
            base.BeCreatedBy(creator);
            creator.TakeEnergy(Template.InitialEnergy);
        }
    }
}
