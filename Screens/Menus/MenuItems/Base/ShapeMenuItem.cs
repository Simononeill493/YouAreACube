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

        public int ScaledWidth => Size.X * Scale;
        public int ScaledHeight => Size.Y * Scale;

        public override void Draw(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawRectangle(Location.X, Location.Y, ScaledWidth, ScaledHeight, DrawLayer, Color);
            base.Draw(drawingInterface);
        }

        public override bool IsMouseOver(UserInput input)
        {
            var output = new Rectangle(Location.X, Location.Y, ScaledWidth, ScaledHeight).Contains(new Microsoft.Xna.Framework.Point(input.MouseX, input.MouseY));
            return output;
        }

        public override Point GetSize()
        {
            return new Point(ScaledWidth, ScaledHeight);
        }
    }
}
