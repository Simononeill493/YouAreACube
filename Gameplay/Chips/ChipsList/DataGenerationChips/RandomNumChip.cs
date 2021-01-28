using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class RandomNumChip : OutputPin<int>, InputPin<int>
    {
        public int ChipInput { get; set; }

        public override void Run(Block actor, UserInput userInput, EffectsList effects)
        {
            SetOutput(RandomUtils.RandomNumber(ChipInput));
        }
    }
}