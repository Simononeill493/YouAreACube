using System;

namespace IAmACube
{
    [Serializable()]
    internal class RandomRelativeChip : OutputPin<RelativeDirection>
    {
        public override void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            SetOutput(RandomUtils.RandomRelative());
        }
    }
}