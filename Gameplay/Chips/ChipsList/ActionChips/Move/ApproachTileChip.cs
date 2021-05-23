using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class ApproachTileChip : InputPin<Tile>
    {
        public string Name { get; set; }
        public Tile ChipInput1 { get; set; }

        public void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            actions.AddApproachAction(actor, ChipInput1);
        }
    }
}