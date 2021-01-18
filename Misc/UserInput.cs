using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class UserInput
    {
        public KeyboardState KeyboardState;
        public MouseState MouseState;

        public List<Keys> KeysJustPressed;
        public List<Keys> KeysJustReleased;

        public UserInput(MouseState mouseState, KeyboardState keyboardState, List<Keys> keysJustPressed, List<Keys> keysJustReleased)
        {
            MouseState = mouseState;
            KeyboardState = keyboardState;
            KeysJustPressed = keysJustPressed;
            KeysJustReleased = keysJustReleased;
        }

        public bool IsKeyDown(Keys key) => KeyboardState.IsKeyDown(key);
        public bool IsKeyUp(Keys key) => KeyboardState.IsKeyUp(key);

        public bool IsKeyJustPressed(Keys key) => KeysJustPressed.Contains(key);
        public bool IsKeyJustReleased(Keys key) => KeysJustReleased.Contains(key);

        public ButtonState LeftButton => MouseState.LeftButton;
        public ButtonState RightButton => MouseState.RightButton;
        public int MouseX => MouseState.X;
        public int MouseY => MouseState.Y;


    }
}
