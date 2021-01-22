using System;

namespace IAmACube
{
    [Serializable()]
    internal class MoveChip : InputPin<Direction>
    {
        public Direction ChipInput { get => _chipInput; set => _chipInput = value; }
        private Direction _chipInput;

        public void Run(Block actor, UserInput userInput)
        {
            actor.Move(ChipInput);
        }
    }
}