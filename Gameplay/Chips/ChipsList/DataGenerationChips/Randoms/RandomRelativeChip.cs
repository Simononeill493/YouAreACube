using System;

namespace IAmACube
{
    [Serializable()]
    internal class RandomRelativeChip : OutputPin<RelativeDirection>
    {
        public string Name { get; set; }

        public RelativeDirection Value { get; set; }

        public void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            Value = (RandomUtils.RandomRelative());
        }
    }
}