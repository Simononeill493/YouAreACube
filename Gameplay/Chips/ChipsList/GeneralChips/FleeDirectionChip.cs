using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class FleeDirectionChip : OutputPin<CardinalDirection>, InputPin<Tile>
    {
        public Tile ChipInput { get; set; }

        public override void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            SetOutput(actor.Location.AbsoluteLocation.FleeDirection(ChipInput.AbsoluteLocation));
        }
    }
}
