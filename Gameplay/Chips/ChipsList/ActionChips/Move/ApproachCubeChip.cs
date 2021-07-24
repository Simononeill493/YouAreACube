using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube.Gameplay.Chips.ChipsList.ActionChips
{
    [Serializable()]
    class ApproachCubeChip<TCube> : InputPin1<TCube> where TCube : Cube
    {
        public string Name { get; set; }
        public TCube ChipInput1 { get; set; }

        public void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actions.AddApproachAction(actor, ChipInput1);
        }
    }
}
