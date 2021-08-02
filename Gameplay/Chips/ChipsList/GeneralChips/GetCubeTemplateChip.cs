using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class GetCubeTemplateChip<TCube> : OutputPin<CubeTemplate>, InputPin1<TCube> where TCube : Cube
    {
        public TCube ChipInput1 { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            SetOutput(Templates.GetRuntimeTemplate(ChipInput1.Template));
        }
    }
}
