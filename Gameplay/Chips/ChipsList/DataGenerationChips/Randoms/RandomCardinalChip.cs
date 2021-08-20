using System;

namespace IAmACube
{
    [Serializable()]
    internal class RandomCardinalChip : OutputPin<CardinalDirection>
    {
        public string Name { get; set; }

        public CardinalDirection Value { get; set; }

        public void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            Value = (RandomUtils.RandomCardinal());
        }
    }
}