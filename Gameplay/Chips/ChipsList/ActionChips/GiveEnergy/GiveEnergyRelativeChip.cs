using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class GiveEnergyRelativeChip : InputPin1<RelativeDirection>, InputPin2<CubeMode>, InputPin3<int>
    {
        public RelativeDirection ChipInput1 { get; set; }
        public CubeMode ChipInput2 { get; set; }
        public int ChipInput3 { get; set; }

        public string Name { get; set; }

        public void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actions.AddGiveEnergyAction(actor, ChipInput2, ChipInput1, ChipInput3);
        }
    }
}
