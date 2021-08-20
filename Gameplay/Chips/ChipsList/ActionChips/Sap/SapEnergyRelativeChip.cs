using System;

namespace IAmACube
{
    [Serializable()]
    internal class SapEnergyRelativeChip : InputPin2<RelativeDirection, CubeMode>
    {

        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actions.AddSapEnergyAction(actor, ChipInput2,ChipInput1);
        }
    }
}