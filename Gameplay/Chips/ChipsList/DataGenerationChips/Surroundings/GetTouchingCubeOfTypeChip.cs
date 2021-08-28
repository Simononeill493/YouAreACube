using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class GetTouchingCubeOfTypeChip : InputPin1<CubeMode>, OutputPin<Cube>
    {
        public Cube Value { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            var target = actor.Location.GetBlock(ChipInput1(actor));

            Value = (target);
        }
    }
}

