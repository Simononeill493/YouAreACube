﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ScreenManager
    {
        public static Screen CurrentScreen;

        public ScreenManager()
        {
        }


        public void Update(MouseState mouseState, KeyboardState keyboardState,List<Keys> keysUp)
        {
            CurrentScreen.Update(mouseState, keyboardState, keysUp);
        }

        public void Draw(DrawingInterface drawingInterface)
        {
            CurrentScreen.Draw(drawingInterface);
        }
    }
}
