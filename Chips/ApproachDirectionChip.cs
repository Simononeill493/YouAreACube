using System;

namespace IAmACube
{
    [Serializable()]
    internal class ApproachDirectionChip : OutputPin<Direction>, InputPin<Tile>
    {
        public Tile ChipInput { get => _input; set => _input = value; }
        private Tile _input;

        public override void Run(Block actor,UserInput userInput)
        {
            SetOutput(actor.Location.ApproachDirection(ChipInput));
        }
    }
}