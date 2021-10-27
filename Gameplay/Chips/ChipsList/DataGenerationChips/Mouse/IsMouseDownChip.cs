using System;

namespace IAmACube
{
    [Serializable()]
    internal class IsMouseDownChip : OutputPin<bool>
    {
        public string Name { get; set; }

        public bool Value { get; set; }

        public void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            Value = userInput.MouseLeftPressed;
        }
    }
}