using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class IsCubeActiveChip<TCube> : InputPin1<TCube>, OutputPin<bool> where TCube : Cube
    {
        public bool Value { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            Value = ChipInput1.Active;
        }
    }
}
