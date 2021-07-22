using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class IsCubeActiveChip<TBlock> : OutputPin<bool>, InputPin<TBlock> where TBlock : Cube
    {
        public TBlock ChipInput1 { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            SetOutput(ChipInput1.Active);
        }
    }
}
