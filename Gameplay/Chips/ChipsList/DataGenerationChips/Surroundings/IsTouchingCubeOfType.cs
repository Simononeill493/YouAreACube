using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class IsTouchingCubeOfTypeChip : InputPin1<CubeMode>, OutputPin<bool>
    {
        public bool Value { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            Value = actor.Location.HasCube(ChipInput1(actor));
        }
    }
}
