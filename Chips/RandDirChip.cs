using System;

namespace IAmACube
{
    [Serializable()]
    internal class RandDirChip : OutputPin<Direction>
    {
        public override void Run(Block actor, UserInput userInput)
        {
            SetOutput(RandomUtils.RandomTestDir());
        }
    }
}