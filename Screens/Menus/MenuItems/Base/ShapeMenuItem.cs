using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ShapeMenuItem : MenuItem
    {
        public Color Color;
        public Point Size;

        public int ScaledWidth => Size.X * Scale;
        public int ScaledHeight => Size.Y * Scale;

        public ShapeMenuItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer) { }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawRectangle(ActualLocation.X, ActualLocation.Y, ScaledWidth, ScaledHeight, DrawLayer, Color);
        }

        public override bool IsMouseOver(UserInput input)
        {
            var output = new Rectangle(ActualLocation.X, ActualLocation.Y, ScaledWidth, ScaledHeight).Contains(new Microsoft.Xna.Framework.Point(input.MouseX, input.MouseY));
            return output;
        }

        public override Point GetSize()
        {
            return new Point(ScaledWidth, ScaledHeight);
        }
    }
}
