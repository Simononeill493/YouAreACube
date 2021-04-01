﻿using System;

namespace IAmACube
{
    [Serializable()]
    internal class MoveCardinalChip : InputPin<CardinalDirection>
    {
        public string Name { get; set; }

        public CardinalDirection ChipInput { get; set; }

        public void Run(Block actor, UserInput userInput,ActionsList actions)
        {
            actions.StartMove(actor, ChipInput);
        }
    }
}