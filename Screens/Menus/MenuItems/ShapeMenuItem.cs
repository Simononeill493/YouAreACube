using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ShapeMenuItem : MenuItem
    {
        public Color Color;
        public int Width;
        public int Height;

        public ShapeMenuItem()
        {
            IsCentered = true;
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawRectangle(LocationOnScreen.X,LocationOnScreen.Y,Width,Height, DrawLayers.MenuItemLayer, Color);

            base.Draw(drawingInterface);
        }

        public override bool IsMouseOver(UserInput input)
        {
            return new Rectangle(LocationOnScreen.X, LocationOnScreen.Y, Width, Height).Contains(new Microsoft.Xna.Framework.Point(input.MouseX, input.MouseY));
        }

        public override void UpdateLocation()
        {
            base.UpdateLocation();
            if (IsCentered)
            {
                LocationOnScreen = LocationOnScreen - new Point(Width / 2, Height / 2);
            }
        }
    }
}
