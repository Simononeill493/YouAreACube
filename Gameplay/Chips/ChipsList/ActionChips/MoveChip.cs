using System;

namespace IAmACube
{
    [Serializable()]
    internal class MoveChip : InputPin<Direction>
    {
        public Direction ChipInput { get; set; }

        public void Run(Block actor, UserInput userInput,EffectsList effects)
        {
            effects.StartMove(actor, ChipInput);
        }
    }
}