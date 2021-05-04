using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class RotateChip : InputPin<int>
    {
        public string Name { get; set; }

        public int ChipInput1 { get; set; }

        public void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            actions.AddRotationAction(actor, ChipInput1);
        }
    }
}
