using System;

namespace IAmACube
{
    [Serializable()]
    internal class ApproachDirectionChip : OutputPin<Direction>, InputPin<Tile>
    {
        public Tile ChipInput { get; set; }

        public override void Run(Block actor,UserInput userInput, EffectsList effects)
        {
            SetOutput(actor.Location.ApproachDirection(ChipInput));
        }
    }
}