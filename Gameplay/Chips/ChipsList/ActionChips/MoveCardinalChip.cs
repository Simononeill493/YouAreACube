using System;

namespace IAmACube
{
    [Serializable()]
    internal class MoveCardinalChip : InputPin<CardinalDirection>
    {
        public CardinalDirection ChipInput { get; set; }

        public void Run(Block actor, UserInput userInput,EffectsList effects)
        {
            effects.StartMove(actor, ChipInput);
        }
    }
}