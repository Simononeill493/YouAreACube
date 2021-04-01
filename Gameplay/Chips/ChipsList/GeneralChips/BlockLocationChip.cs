using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class BlockLocationChip : OutputPin<Tile>, InputPin<SurfaceBlock>
    {
        public SurfaceBlock ChipInput1 { get; set; }

        public override void Run(Block actor, UserInput input, ActionsList actions)
        {
            SetOutput(ChipInput1.Location);
        }
    }
}
