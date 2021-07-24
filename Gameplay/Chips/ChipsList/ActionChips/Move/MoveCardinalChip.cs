using System;

namespace IAmACube
{
    [Serializable()]
    internal class MoveCardinalChip : InputPin1<CardinalDirection>
    {
        public string Name { get; set; }

        public CardinalDirection ChipInput1 { get; set; }

        public void Run(Cube actor, UserInput userInput,ActionsList actions)
        {
            actions.AddMoveAction(actor, ChipInput1);
        }
    }
}