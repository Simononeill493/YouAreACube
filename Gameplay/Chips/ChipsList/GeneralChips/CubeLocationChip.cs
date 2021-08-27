using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class CubeLocationChip<TBlock> : InputPin1<TBlock>, OutputPin<Tile> where TBlock : Cube
    {
        public Tile Value { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            var target = ChipInput1(actor);
            if(target!=null)
            {
                Value = (target.Location);
            }
        }
    }
}
