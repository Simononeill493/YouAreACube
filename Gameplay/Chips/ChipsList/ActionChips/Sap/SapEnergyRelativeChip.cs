using System;

namespace IAmACube
{
    [Serializable()]
    internal class SapEnergyRelativeChip : InputPin1<RelativeDirection>, InputPin2<CubeMode>
    {
        public string Name { get; set; }

        public RelativeDirection ChipInput1 { get; set; }
        public CubeMode ChipInput2 { get; set; }

        public void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actions.AddSapEnergyAction(actor, ChipInput2,ChipInput1);
        }
    }
}