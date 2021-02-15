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
            Dimensions.IsCentered = true;
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawRectangle(X, Y,Width,Height, DrawLayers.MenuItemLayer, Color,Dimensions.IsCentered);

            base.Draw(drawingInterface);
        }
        public override bool IsMouseOver(UserInput input)
        {
            var (x, y) = Dimensions.ActualLocation;
            if(Dimensions.IsCentered)
            {
                (x, y) = DrawUtils.GetCenteredCoords(Width, Height, x, y, 1);
            }

            var output = new Rectangle(x, y, Width, Height).Contains(new Microsoft.Xna.Framework.Point(input.MouseX, input.MouseY));
            return output;
        }
    }
}
