using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class RectangleScreenItem : ScreenItem
    {
        public Color Color;

        public IntPoint Size;

        public int ScaledWidth => Size.X * Scale;
        public int ScaledHeight => Size.Y * Scale;

        public RectangleScreenItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer) { }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawRectangle(ActualLocation.X, ActualLocation.Y, ScaledWidth, ScaledHeight, DrawLayer, Color);
        }
        public override IntPoint GetBaseSize() => Size;
    }
}
