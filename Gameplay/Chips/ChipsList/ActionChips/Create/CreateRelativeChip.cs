using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class CreateRelativeChip : InputPin<RelativeDirection>, InputPin2<CubeTemplate>, InputPin3<CubeMode>
    {
        public string Name { get; set; }

        public RelativeDirection ChipInput1 { get; set; }
        public CubeTemplate ChipInput2 { get; set; }
        public CubeMode ChipInput3 { get; set; }

        public void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actions.AddCreationAction(actor, ChipInput2, ChipInput3, ChipInput1);
        }
    }
}
