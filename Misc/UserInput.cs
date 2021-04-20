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
        private static int previousScrollValue;

        public bool ScrolledUp;
        public bool ScrolledDown;

        public KeyboardState KeyboardState;
        public MouseState MouseState;

        public List<Keys> KeysJustPressed;
        public List<Keys> KeysJustReleased;

        public UserInput(MouseState mouseState, MouseState oldMouseState,KeyboardState keyboardState, List<Keys> keysJustPressed, List<Keys> keysJustReleased)
        {
            MouseState = mouseState;
            KeyboardState = keyboardState;
            KeysJustPressed = keysJustPressed;
            KeysJustReleased = keysJustReleased;

            ScrolledUp = MouseState.ScrollWheelValue > previousScrollValue;
            ScrolledDown = MouseState.ScrollWheelValue < previousScrollValue;

            previousScrollValue = MouseState.ScrollWheelValue;

            MousePos = new Point(MouseX, MouseY);

            MouseLeftPressed = MouseState.LeftButton == ButtonState.Pressed;
            MouseRightPressed = MouseState.RightButton == ButtonState.Pressed;

            MouseLeftReleased = MouseState.LeftButton == ButtonState.Released;
            MouseRightReleased = MouseState.RightButton == ButtonState.Released;

            MouseLeftJustPressed = MouseLeftPressed & (oldMouseState.LeftButton == ButtonState.Released);
            MouseRightJustPressed = MouseRightPressed & (oldMouseState.RightButton == ButtonState.Released);
        }

        public bool IsKeyDown(Keys key) => KeyboardState.IsKeyDown(key);
        public bool IsKeyUp(Keys key) => KeyboardState.IsKeyUp(key);

        public bool IsKeyJustPressed(Keys key) => KeysJustPressed.Contains(key);
        public bool IsKeyJustReleased(Keys key) => KeysJustReleased.Contains(key);

        public void RemoveKeyJustPressed(Keys key) => KeysJustPressed.Remove(key);
        public void RemoveKeyJustReleased(Keys key) => KeysJustReleased.Remove(key);

        public bool MouseLeftJustPressed;
        public bool MouseRightJustPressed;

        public bool MouseLeftPressed;
        public bool MouseRightPressed;

        public bool MouseLeftReleased;
        public bool MouseRightReleased;

        public int MouseX => MouseState.X;
        public int MouseY => MouseState.Y;

        public Point MousePos;
        public Tile MouseHoverTile = Tile.Dummy;
    }
}
