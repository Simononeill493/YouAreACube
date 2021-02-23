using System;

namespace IAmACube
{
    [Serializable()]
    internal class ApproachDirectionChip : OutputPin<CardinalDirection>, InputPin<Tile>
    {
        public Tile ChipInput { get; set; }

        public override void Run(Block actor,UserInput userInput, ActionsList actions)
        {
            SetOutput(actor.Location.AbsoluteLocation.ApproachDirection(ChipInput.AbsoluteLocation));
        }
    }
}