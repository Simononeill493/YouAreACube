﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Runtime.InteropServices;
using System.IO;
using wf = System.Windows.Forms;
using System.Collections.Generic;

namespace IAmACube
{
    public class MonoGameWindow : Microsoft.Xna.Framework.Game
    {
        public static IntPoint CurrentSize;

        private DrawingInterface _drawingInterface;
        private PrimitivesHelper _primitivesHelper;

        private GraphicsDeviceManager _graphicsDeviceManager;
        private AttachedConsoleManager _attachedConsoleManager;
        private ScreenManager _screenManager;

        private KeyboardState _currentKeyboardState;

        FrameCounter _frameCounter = new FrameCounter();

        public MonoGameWindow()
        {
            #region Boilerplate
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Exiting += WhenGameClosed;
            Window.AllowUserResizing = true;

            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            _setScreenSizeToDefault();
            #endregion

            CurrentSize = new IntPoint(_graphicsDeviceManager.GraphicsDevice.Viewport.Width, _graphicsDeviceManager.GraphicsDevice.Viewport.Height);
            oldInput = new UserInput(Mouse.GetState(), Mouse.GetState(), Keyboard.GetState(), new List<Keys>(), new List<Keys>());
            _attachedConsoleManager = new AttachedConsoleManager(GraphicsDevice,Window.Position);
            this.Window.ClientSizeChanged += _attachedConsoleManager.ConsoleMoveEventHandler;

            _primitivesHelper = new PrimitivesHelper();
            _drawingInterface = new DrawingInterface(_primitivesHelper);

            _screenManager = new ScreenManager();

            _currentKeyboardState = Keyboard.GetState();

            Window.Position = new Microsoft.Xna.Framework.Point(16, 32);
        }


        protected override void Update(GameTime gameTime)
        {
            _attachedConsoleManager.CheckWindowPositionAndUpdateConsole(Window.Position);
            var mouseState = Mouse.GetState();
            var newKeyState = Keyboard.GetState();

            var previouslyPressed = _currentKeyboardState.GetPressedKeys();
            var currentlyPressed = newKeyState.GetPressedKeys();

            var keysUp = new List<Keys>();
            var keysDown = new List<Keys>();

            foreach (var key in previouslyPressed)
            {
                if(!newKeyState.IsKeyDown(key))
                {
                    keysUp.Add(key);
                }
            }
            foreach (var key in currentlyPressed)
            {
                if (!_currentKeyboardState.IsKeyDown(key))
                {
                    keysDown.Add(key);
                }
            }

            _currentKeyboardState = newKeyState;
            var input = new UserInput(mouseState,oldInput.MouseState, _currentKeyboardState, keysDown, keysUp);

            _screenManager.Update(input);
            base.Update(gameTime);

            oldInput = input;
        }

        UserInput oldInput;

        private void _setScreenSizeToDefault()
        {
            _graphicsDeviceManager.PreferredBackBufferWidth = Config.ScreenDefaultWidth; //Set the screen size.
            _graphicsDeviceManager.PreferredBackBufferHeight = Config.ScreenDefaultHeight;
            _graphicsDeviceManager.ApplyChanges();
        }

        protected override void Draw(GameTime gameTime)
        {
            if(Config.EnableFrameCounter)
            {
                _frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                Console.Write(_frameCounter.CurrentFramesPerSecond);
                Console.Write((char)13);
            }

            CurrentSize = new IntPoint(_graphicsDeviceManager.GraphicsDevice.Viewport.Width, _graphicsDeviceManager.GraphicsDevice.Viewport.Height);
            _primitivesHelper.BeginDrawFrame();
            _screenManager.Draw(_drawingInterface);
            _primitivesHelper.EndDrawFrame();

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            _primitivesHelper.LoadContent(GraphicsDevice, Content);
            _screenManager.Init();

            _setScreenSizeToDefault();
        }

        protected override void UnloadContent()
        {

        }

        private void WhenGameClosed(object sender, EventArgs e)
        //Implemented to deal with a bug where window freezes when close button is clicked
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
