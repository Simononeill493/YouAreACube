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
                if(!destination.Ephemeral.ShouldBeDestroyed())
                {
                    var target = destination.Ephemeral;
                    Console.WriteLine("Ephemeral " + _id + " at " + Location.AbsoluteLocation + " absorbed into Ephemeral " + target._id + " at " + target.Location.AbsoluteLocation);
                    AbsorbInto(destination.Ephemeral);
                    return;
                }
            }

            if (!Location.HasThisEphemeral(this))
            {
                throw new Exception("Block trying to exit a location despite not existing in that location");
            }

            Location.Ephemeral = null;
            this.Location = destination;
            Location.Ephemeral = this;
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
