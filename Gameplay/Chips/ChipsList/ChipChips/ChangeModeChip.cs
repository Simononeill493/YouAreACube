using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube.Gameplay.Chips.ChipsList.ChipChips
{
    namespace IAmACube
    {
        [Serializable()]
        class ChangeModeChip: InputPin1<int>
        {
            public override void Run(Cube actor, UserInput userInput, ActionsList actions)
            {
                var modeNum = ChipInput1(actor);
                actions.AddModeChangeAction(actor, modeNum);
            }
        }
    }
}
