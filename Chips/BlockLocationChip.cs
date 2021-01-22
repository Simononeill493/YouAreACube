using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class BlockLocationChip : OutputPin<Tile>, InputPin<Block>
    {
        public Block ChipInput { get => _input; set => _input = value; }
        private Block _input;

        public override void Run(Block actor, UserInput input)
        {
            SetOutput(ChipInput.Location);
        }
    }
}
