using System;

namespace IAmACube
{
    [Serializable()]
    internal class SapEnergyCardinalChip :InputPin2<CardinalDirection,CubeMode>
    {
        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actions.AddSapEnergyAction(actor, ChipInput2(actor), ChipInput1(actor));
        }
    }
}