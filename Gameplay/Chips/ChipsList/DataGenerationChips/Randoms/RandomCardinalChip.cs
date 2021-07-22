using System;

namespace IAmACube
{
    [Serializable()]
    internal class RandomCardinalChip : OutputPin<CardinalDirection>
    {
        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            SetOutput(RandomUtils.RandomCardinal());
        }
    }
}