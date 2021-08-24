using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class RandomNumChip : InputPin1<int>,OutputPin<int>
    {
        public int Value { get; set; }

        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            Value = (RandomUtils.RandomNumber(ChipInput1(actor)));
        }
    }
}