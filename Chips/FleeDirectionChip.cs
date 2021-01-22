using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class FleeDirectionChip : OutputPin<Direction>, InputPin<Tile>
    {
        public Tile ChipInput { get => _input; set => _input = value; }
        private Tile _input;

        public override void Run(Block actor, UserInput userInput)
        {
            SetOutput(actor.Location.FleeDirection(ChipInput));
        }
    }
}
