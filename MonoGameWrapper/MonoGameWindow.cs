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
namespace IAmACube
{
    public class MonoGameWindow : Microsoft.Xna.Framework.Game
    {
        public static int CurrentWidth;
        public static int CurrentHeight;

        private DrawingInterface _drawingInterface;
        private GraphicsDeviceManager _graphicsDeviceManager;
        private AttachedConsoleManager _attachedConsoleManager;
        private ScreenManager _screenManager;

        FrameCounter _frameCounter = new FrameCounter();

        public MonoGameWindow()
        {
            #region Boilerplate
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Exiting += WhenGameClosed;
            Window.AllowUserResizing = true;

            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            _graphicsDeviceManager.PreferredBackBufferWidth = 500; //Set the screen size.
            _graphicsDeviceManager.PreferredBackBufferHeight = 500;
            _graphicsDeviceManager.ApplyChanges();
            #endregion

            _attachedConsoleManager = new AttachedConsoleManager(GraphicsDevice,Window.Position);
            this.Window.ClientSizeChanged += _attachedConsoleManager.ConsoleMoveEventHandler;

            _drawingInterface = new DrawingInterface();
            _screenManager = new ScreenManager();
            ScreenManager.CurrentScreen = new TitleScreen();
        }

        protected override void Update(GameTime gameTime)
        {
            _attachedConsoleManager.CheckWindowPositionAndUpdateConsole(Window.Position);
            var mouseState = Mouse.GetState();
            var keyState = Keyboard.GetState();

            _screenManager.Update(mouseState, keyState);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if(Config.EnableFrameCounter)
            {
                _frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                Console.Write(_frameCounter.CurrentFramesPerSecond);
                Console.Write((char)13);
            }

            _drawingInterface.graphicsDevice = _graphicsDeviceManager.GraphicsDevice;
            CurrentWidth = _graphicsDeviceManager.GraphicsDevice.Viewport.Width;
            CurrentHeight = _graphicsDeviceManager.GraphicsDevice.Viewport.Height;

            _drawingInterface.BeginDrawFrame();
            _screenManager.Draw(_drawingInterface);
            _drawingInterface.EndDrawFrame();
            base.Draw(gameTime);

        }

        protected override void LoadContent()
        {
            _drawingInterface.LoadContent(GraphicsDevice, Content);
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
