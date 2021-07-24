using System;

namespace IAmACube
{
    [Serializable()]
    internal class ApproachDirectionChip : OutputPin<CardinalDirection>, InputPin1<Tile>
    {
        public Tile ChipInput1 { get; set; }

        public override void Run(Cube actor,UserInput userInput, ActionsList actions)
        {
            SetOutput(actor.Location.AbsoluteLocation.ApproachDirection(ChipInput1.AbsoluteLocation));
        }
    }
}