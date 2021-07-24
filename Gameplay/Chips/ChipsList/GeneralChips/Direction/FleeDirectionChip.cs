using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class FleeDirectionChip : OutputPin<CardinalDirection>, InputPin1<Tile>
    {
        public Tile ChipInput1 { get; set; }

        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            SetOutput(actor.Location.AbsoluteLocation.FleeDirection(ChipInput1.AbsoluteLocation));
        }
    }
}
