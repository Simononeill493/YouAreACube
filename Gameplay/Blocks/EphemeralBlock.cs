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
            BlockType = BlockType.Ephemeral;
        }

        public override void Update(UserInput input, ActionsList actions)
        {
            base.Update(input, actions);
            Energy--;
        }

        protected override void _move(Tile destination)
        {
            Location.Ephemeral = null;
            destination.Ephemeral = this;
            this.Location = destination;
        }

        public override bool CanOccupyDestination(Tile destination)
        {
            return !destination.HasEphemeral;
        }
    }
}
