using System;

namespace IAmACube
{
    [Serializable()]
    internal class RandomCardinalChip : OutputPin<CardinalDirection>
    {
        public override void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            SetOutput(RandomUtils.RandomCardinal());
        }
    }
}