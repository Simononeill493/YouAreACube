using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace IAmACube
{
    class CorneredBoxMenuItem : RectangleScreenItem
    {
        public const string CornerSprite = "DialogueBoxEdge";

        public static Color LineColor = new Color(31, 82, 240);
        public static Color BackgroundColor = new Color(210, 226, 255);

        protected IntPoint _cornerSize;
        public override float Alpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                _alpha = value;
                _corners.ForEach(c => c.Alpha = value);
            }
        }
        private float _alpha;

        private List<SpriteScreenItem> _corners = new List<SpriteScreenItem>();

        public CorneredBoxMenuItem(IHasDrawLayer parent,IntPoint size) : base(parent)
        {
            RectangleSizePixels = size;
            _cornerSize = SpriteManager.GetSpriteSize(CornerSprite);

            var topLeft = new SpriteScreenItem(this, CornerSprite);
            topLeft.SetLocationConfig(0, 0, CoordinateMode.ParentPixel,centered: false);
            AddChild(topLeft);

            var topRight = new SpriteScreenItem(this, CornerSprite);
            topRight.SetLocationConfig(RectangleSizePixels.X- _cornerSize.X, 0, CoordinateMode.ParentPixel, centered: false);
            topRight.FlipHorizontal = true;
            AddChild(topRight);

            var bottomLeft = new SpriteScreenItem(this, CornerSprite);
            bottomLeft.SetLocationConfig(0, RectangleSizePixels.Y - _cornerSize.Y, CoordinateMode.ParentPixel, centered: false);
            bottomLeft.FlipVertical = true;
            AddChild(bottomLeft);

            var bottomRight = new SpriteScreenItem(this, CornerSprite);
            bottomRight.SetLocationConfig(RectangleSizePixels.X - _cornerSize.X, RectangleSizePixels.Y - _cornerSize.Y, CoordinateMode.ParentPixel, centered: false);
            bottomRight.FlipHorizontal = true;
            bottomRight.FlipVertical = true;
            AddChild(bottomRight);

            _corners.Add(topLeft);
            _corners.Add(topRight);
            _corners.Add(bottomLeft);
            _corners.Add(bottomRight);
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            var scale = Scale;
            var curSize = GetCurrentSize();
            var cornerSizeCurrent = _cornerSize * scale;
            var afterCorner = ActualLocation + cornerSizeCurrent;
            var horizontalBarWidth = curSize.X - (cornerSizeCurrent.X * 2);
            var verticalBarHeight = curSize.Y - (cornerSizeCurrent.Y * 2);
            var internalOriginLoc = ActualLocation + curSize - cornerSizeCurrent;

            //Internal Box
            drawingInterface.DrawRectangle(afterCorner.X,                           afterCorner.Y,                               horizontalBarWidth,             verticalBarHeight,              DrawLayer, BackgroundColor * Alpha);

            //Outer Lines
            drawingInterface.DrawRectangle(afterCorner.X,                           ActualLocation.Y,                           horizontalBarWidth,             scale,                          DrawLayer, LineColor * Alpha);
            drawingInterface.DrawRectangle(afterCorner.X,                           ActualLocation.Y + curSize.Y- scale,        horizontalBarWidth,             scale,                          DrawLayer, LineColor * Alpha);
            drawingInterface.DrawRectangle(ActualLocation.X,                        afterCorner.Y,                              scale,                          verticalBarHeight,              DrawLayer, LineColor * Alpha);
            drawingInterface.DrawRectangle(ActualLocation.X + curSize.X - scale,    afterCorner.Y,                              scale,                          verticalBarHeight,              DrawLayer, LineColor * Alpha);

            //Inner Lines
            drawingInterface.DrawRectangle(afterCorner.X,                           ActualLocation.Y + scale,                   horizontalBarWidth,             cornerSizeCurrent.X-scale,      DrawLayer, BackgroundColor * Alpha);
            drawingInterface.DrawRectangle(afterCorner.X,                           internalOriginLoc.Y,                        horizontalBarWidth,             cornerSizeCurrent.X-(scale),    DrawLayer, BackgroundColor * Alpha);
            drawingInterface.DrawRectangle(ActualLocation.X + scale,                afterCorner.Y,                              cornerSizeCurrent.Y-scale,      verticalBarHeight,              DrawLayer, BackgroundColor * Alpha);
            drawingInterface.DrawRectangle(internalOriginLoc.X,                     afterCorner.Y,                              cornerSizeCurrent.Y-(scale),    verticalBarHeight,              DrawLayer, BackgroundColor * Alpha);
        }
    }
}
