using System;

namespace IAmACube
{
    [Serializable()]
    internal class ApproachDirectionChip : OutputPin<CardinalDirection>, InputPin<Tile>
    {
        public Tile ChipInput1 { get; set; }

        public override void Run(Block actor,UserInput userInput, ActionsList actions)
        {
            var output = actor.Location.AbsoluteLocation.ApproachDirection(ChipInput1.AbsoluteLocation);

            SetOutput(output);
            Console.WriteLine(ChipInput1);
        }
    }
}