using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class RandomNumChip : OutputPin<int>, InputPin1<int>
    {
        public int ChipInput1 { get; set; }

        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            SetOutput(RandomUtils.RandomNumber(ChipInput1));
        }
    }
}