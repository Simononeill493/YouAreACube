using System;

namespace IAmACube
{
    [Serializable()]
    internal class MoveCardinalChip : InputPin1<CardinalDirection>
    {

        public override void Run(Cube actor, UserInput userInput,ActionsList actions)
        {
            actions.AddMoveAction(actor, ChipInput1(actor));
        }
    }
}