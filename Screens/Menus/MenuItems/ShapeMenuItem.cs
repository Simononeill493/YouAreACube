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
        public Point Size;

        public ShapeMenuItem()
        {
            Dimensions.IsCentered = true;
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            var sizeScaled = Size * Dimensions.Scale;

            drawingInterface.DrawRectangle(X, Y, sizeScaled.X, sizeScaled.Y, DrawLayers.MenuItemLayer, Color,Dimensions.IsCentered);
            base.Draw(drawingInterface);
        }

        public override bool IsMouseOver(UserInput input)
        {
            var (x, y) = Dimensions.ActualLocation;
            var sizeScaled = Size * Dimensions.Scale;

            if(Dimensions.IsCentered)
            {
                (x, y) = DrawUtils.GetCenteredCoords(sizeScaled.X, sizeScaled.Y, x, y, 1);
            }

            var output = new Rectangle(x, y, sizeScaled.X, sizeScaled.Y).Contains(new Microsoft.Xna.Framework.Point(input.MouseX, input.MouseY));

            /*if(output)
            {
                Console.WriteLine("Yes");
            }
            else
            {
                Console.WriteLine("No");
            }*/

            return output;
        }
    }
}
