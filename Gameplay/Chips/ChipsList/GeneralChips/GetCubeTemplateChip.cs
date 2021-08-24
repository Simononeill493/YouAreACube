using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class GetCubeTemplateChip<TCube> : InputPin1<TCube>, OutputPin<CubeTemplate> where TCube : Cube
    {
        public CubeTemplate Value { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            Value = (ChipInput1(actor).Template.GetRuntimeTemplate());
        }
    }
}
