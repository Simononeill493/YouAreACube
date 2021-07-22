using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class ZapChip : InputPin<CubeMode>
    {
        public string Name { get; set; }
        public CubeMode ChipInput1 { get; set; }

        public void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actions.AddZapAction(actor, ChipInput1);
        }
    }
}
