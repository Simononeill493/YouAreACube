using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class IsKeyPressedChip : InputPin1<Keys>, OutputPin<bool>
    {
        public bool Value { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            Value = (input.KeyboardState.IsKeyDown(ChipInput1(actor)));
        }
    }
}
