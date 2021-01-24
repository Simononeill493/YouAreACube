using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class IsKeyPressedChip : OutputPin<bool>, InputPin<Keys>
    {
        public Keys ChipInput { get; set; }

        public override void Run(Block actor, UserInput input, EffectsList effects)
        {
            SetOutput(input.KeyboardState.IsKeyDown(ChipInput));
        }
    }
}
