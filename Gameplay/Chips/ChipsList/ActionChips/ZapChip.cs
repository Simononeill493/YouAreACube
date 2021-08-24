using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class ZapChip : InputPin1<CubeMode>
    {
        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actions.AddZapAction(actor, ChipInput1(actor));
        }
    }
}
