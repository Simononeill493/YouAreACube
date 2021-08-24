using System;

namespace IAmACube
{
    [Serializable()]
    internal class ApproachDirectionChip : InputPin1<Tile>, OutputPin<CardinalDirection>
    {
        public CardinalDirection Value { get; set; }

        public override void Run(Cube actor,UserInput userInput, ActionsList actions)
        {
            Value = (actor.Location.AbsoluteLocation.ApproachDirection(ChipInput1(actor).AbsoluteLocation));
        }
    }
}