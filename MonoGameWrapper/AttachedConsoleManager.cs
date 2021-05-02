using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class AttachedConsoleManager
    {
        private GraphicsDevice _graphicsDevice;
        private Point _lastRecordedPosition;

        public AttachedConsoleManager(GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Point firstPosition)
        {
            _graphicsDevice = graphicsDevice;
            _lastRecordedPosition = firstPosition;

            Console.CursorVisible = false;
        }

        public void ConsoleMoveEventHandler(object sender, EventArgs e) => MoveConsoleToWindow();
        public void MoveConsoleToWindow()
        {
            var bounds = _graphicsDevice.PresentationParameters.Bounds;
            MoveWindow(GetConsoleWindow(), _lastRecordedPosition.X + bounds.Width, _lastRecordedPosition.Y, 400, bounds.Height, true);
        }
        public void CheckWindowPositionAndUpdateConsole(Point currentPosition)
        {
            if(currentPosition!=_lastRecordedPosition)
            {
                _lastRecordedPosition = currentPosition;
                MoveConsoleToWindow();
            }
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
    }
}
