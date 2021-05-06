using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class BlockLocationChip<TBlock> : OutputPin<Tile>, InputPin<TBlock> where TBlock : Block 
    {
        public TBlock ChipInput1 { get; set; }

        public override void Run(Block actor, UserInput input, ActionsList actions)
        {
            SetOutput(ChipInput1.Location);
        }
    }
}
