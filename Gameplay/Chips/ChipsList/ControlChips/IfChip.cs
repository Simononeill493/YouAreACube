using System;

namespace IAmACube
{
    [Serializable()]
    internal class IfChip : InputPin<bool>, IControlChip
    {
        public bool ChipInput { get; set; }
        public ChipBlock Result { get; set; }

        public ChipBlock Yes;
        public ChipBlock No;

        public void Run(Block actor,UserInput input)
        {
            if(ChipInput)
            {
                Result = Yes;
                return;
            }

            Result = No;
        }

    }
}