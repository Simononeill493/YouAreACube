using System;

namespace IAmACube
{
    [Serializable()]
    internal class IfChip : InputPin<bool>, IControlChip
    {
        public bool ChipInput { get => _input; set => _input = value; }
        private bool _input;

        public ChipBlock Result { get => _result; set => _result = value; }
        private ChipBlock _result;

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