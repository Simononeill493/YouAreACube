using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class ZapChip : InputPin<BlockMode>
    {
        public string Name { get; set; }
        public BlockMode ChipInput1 { get; set; }

        public void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            actions.AddZapAction(actor, ChipInput1);
        }
    }
}
