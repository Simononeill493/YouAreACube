using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class ApproachTileChip : InputPin1<Tile>
    {
        public string Name { get; set; }
        public Tile ChipInput1 { get; set; }

        public void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actions.AddApproachAction(actor, ChipInput1);
        }
    }
}