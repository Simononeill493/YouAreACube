using System;

namespace IAmACube
{
    [Serializable()]
    internal class IfChip : IControlChip, InputPin<bool> 
    {
        public bool ChipInput { get; set; }
        public ChipBlock Result;
        public ChipBlock Yes;
        public ChipBlock No;

        public void Run(Block actor,UserInput input,ActionsList actions)
        {
            if(ChipInput)
            {
                Result = Yes;
                return;
            }

            Result = No;
        }

        public void ExecuteOutput(Block actor, UserInput input, ActionsList actions)
        {
            Result.Execute(actor, input, actions);
        }
    }
}