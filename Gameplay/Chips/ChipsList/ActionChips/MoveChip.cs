using System;

namespace IAmACube
{
    [Serializable()]
    internal class MoveChip : InputPin<Direction>
    {
        public Direction ChipInput { get; set; }

        public void Run(Block actor, UserInput userInput)
        {
            actor.Move(ChipInput);
        }
    }
}