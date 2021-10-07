using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class RectangleScreenItem : VisualScreenItem
    {
        public IntPoint RectangleSizePixels;

        public int ScaledWidth => RectangleSizePixels.X * Scale;
        public int ScaledHeight => RectangleSizePixels.Y * Scale;

        public RectangleScreenItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer) { }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawRectangle(ActualLocation.X, ActualLocation.Y, ScaledWidth, ScaledHeight, DrawLayer, CurrentColor * Alpha);
        }
        public override IntPoint GetBaseSize() => RectangleSizePixels;
    }
}
