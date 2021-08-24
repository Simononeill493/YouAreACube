using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class FleeDirectionChip : InputPin1<Tile>, OutputPin<CardinalDirection>
    {
        public CardinalDirection Value { get; set; }

        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            Value =(actor.Location.AbsoluteLocation.FleeDirection(ChipInput1(actor).AbsoluteLocation));
        }
    }
}
