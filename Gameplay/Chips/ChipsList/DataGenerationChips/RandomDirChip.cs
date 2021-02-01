using System;

namespace IAmACube
{
    [Serializable()]
    internal class RandomDirChip : OutputPin<CardinalDirection>
    {
        public override void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            SetOutput(RandomUtils.RandomDirection());
        }
    }
}