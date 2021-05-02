using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public partial class UserInput
    {
        public UserInput(MouseState mouseState, MouseState oldMouseState,KeyboardState keyboardState, KeyboardState oldKeyboardState)
        {
            _setMouseState(mouseState, oldMouseState);
            _setKeyboardState(keyboardState, oldKeyboardState);
        }
    }
}
