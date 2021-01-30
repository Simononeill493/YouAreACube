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

        public void Run(Block actor,UserInput input,EffectsList effects)
        {
            if(ChipInput)
            {
                Result = Yes;
                return;
            }

            Result = No;
        }

        public void ExecuteOutput(Block actor, UserInput input, EffectsList effects)
        {
            Result.Execute(actor, input, effects);
        }
    }
}