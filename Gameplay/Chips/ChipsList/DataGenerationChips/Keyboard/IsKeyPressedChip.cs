using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class IsKeyPressedChip : OutputPin<bool>, InputPin1<Keys>
    {
        public Keys ChipInput1 { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            SetOutput(input.KeyboardState.IsKeyDown(ChipInput1));
        }
    }
}
