using Microsoft.Xna.Framework;
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
        private DrawingInterfacePrimitive _primitivesHelper;

        private GraphicsDeviceManager _graphicsDeviceManager;
        private AttachedConsoleManager _attachedConsoleManager;
        private ScreenManager _screenManager;

        private FrameCounter _frameCounter = new FrameCounter();
        private UserInput _previousInput;

        public MonoGameWindow()
        {
            _setWindowData();

            _previousInput = new UserInput(Mouse.GetState(), Mouse.GetState(), Keyboard.GetState(), Keyboard.GetState());
            _primitivesHelper = new DrawingInterfacePrimitive();
            _drawingInterface = new DrawingInterface(_primitivesHelper);
            _screenManager = new ScreenManager();
        }

        protected override void Update(GameTime gameTime)
        {
            _attachedConsoleManager.CheckWindowPositionAndUpdateConsole(Window.Position);
            var input = new UserInput(Mouse.GetState(), _previousInput.MouseState, Keyboard.GetState(), _previousInput.KeyboardState);

            _screenManager.Update(input);
            base.Update(gameTime);

            _previousInput = input;
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


        private void _setWindowData()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Exiting += WhenGameClosed;
            Window.AllowUserResizing = true;

            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            _setScreenSizeToDefault();
            CurrentSize = new IntPoint(_graphicsDeviceManager.GraphicsDevice.Viewport.Width, _graphicsDeviceManager.GraphicsDevice.Viewport.Height);

            _attachedConsoleManager = new AttachedConsoleManager(GraphicsDevice, Window.Position);
            this.Window.ClientSizeChanged += _attachedConsoleManager.ConsoleMoveEventHandler;
            Window.Position = new Microsoft.Xna.Framework.Point(16, 32);
        }
        private void _setScreenSizeToDefault()
        {
            _graphicsDeviceManager.PreferredBackBufferWidth = Config.ScreenDefaultWidth; //Set the screen size.
            _graphicsDeviceManager.PreferredBackBufferHeight = Config.ScreenDefaultHeight;
            _graphicsDeviceManager.ApplyChanges();
        }
        private void WhenGameClosed(object sender, EventArgs e)
        //Implemented to deal with a bug where window freezes when close button is clicked
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
