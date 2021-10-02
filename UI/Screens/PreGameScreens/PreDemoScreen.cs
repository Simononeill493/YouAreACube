using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class PreDemoScreen : MenuScreen
    {
        public const string DemoBuddy = "DemoBuddy";

        public PreDemoScreen(Action<ScreenType> switchScreen) : base(ScreenType.PreDemo, switchScreen)
        {
            Background = MenuSprites.MainMenuBox;
            //_manualResizeEnabled = false;

            var buddy = new SpriteScreenItem(this, DemoBuddy);
            buddy.SetLocationConfig(0, 0, CoordinateMode.ParentPixel, false);
            buddy.MultiplyScale(4.0f);
            buddy.AddAnimation(new MoveToPixelOffsetAnimation(Triggers.Timed(120), Tickers.Cyclic(0), new IntPoint(0, -10)));

            var dialogue = new CorneredBox(this, new IntPoint(80, 30));
            dialogue.SetLocationConfig(-13, -20, CoordinateMode.ParentPixel, true);
            dialogue.MultiplyScale(0.75f);
            dialogue.AddAnimation(new TextBoxFadeInAnimation(Triggers.Timed(180), Tickers.Constant,0.01f));
            buddy.AddChild(dialogue);

            var buddyHolder = new ContainerScreenItem(this);
            buddyHolder.SetLocationConfig(65, 100, CoordinateMode.ParentPercentage, false);
            buddyHolder.AddChild(buddy);
            //buddyHolder.AddChild(dialogue);
            _addMenuItem(buddyHolder);
        }
    }

    public class CorneredBox : RectangleScreenItem
    {
        public const string CornerSprite = "DialogueBoxEdge";
        public const string PointSprite = "DialogueBoxPoint";

        public static Color LineColor = new Color(31, 82, 240);
        public static Color BackgroundColor = new Color(210, 226, 255);

        private IntPoint _cornerSize;
        public float Alpha
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

        public CorneredBox(IHasDrawLayer parent,IntPoint size) : base(parent)
        {
            Size = size;
            _cornerSize = SpriteManager.GetSpriteSize(CornerSprite);

            var topLeft = new SpriteScreenItem(this, CornerSprite);
            topLeft.SetLocationConfig(0, 0, CoordinateMode.ParentPixel,centered: false);
            AddChild(topLeft);

            var topRight = new SpriteScreenItem(this, CornerSprite);
            topRight.SetLocationConfig(Size.X- _cornerSize.X, 0, CoordinateMode.ParentPixel, centered: false);
            topRight.FlipHorizontal = true;
            AddChild(topRight);

            var bottomLeft = new SpriteScreenItem(this, CornerSprite);
            bottomLeft.SetLocationConfig(0, Size.Y - _cornerSize.Y, CoordinateMode.ParentPixel, centered: false);
            bottomLeft.FlipVertical = true;
            AddChild(bottomLeft);

            var bottomRight = new SpriteScreenItem(this, CornerSprite);
            bottomRight.SetLocationConfig(Size.X - _cornerSize.X, Size.Y - _cornerSize.Y, CoordinateMode.ParentPixel, centered: false);
            bottomRight.FlipHorizontal = true;
            bottomRight.FlipVertical = true;
            AddChild(bottomRight);

            _corners.Add(topLeft);
            _corners.Add(topRight);
            _corners.Add(bottomLeft);
            _corners.Add(bottomRight);

            Alpha = 0;
        }


        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            var scale = Scale;
            var curSize = GetCurrentSize();
            var cornerSizeCurrent = _cornerSize * scale;

            var innerOffset = scale * 2;
            drawingInterface.DrawRectangle(ActualLocation.X+(innerOffset), ActualLocation.Y+(innerOffset), ScaledWidth-(innerOffset * 2), ScaledHeight-(innerOffset * 2), DrawLayer, BackgroundColor*Alpha);

            var afterCornerX = ActualLocation.X + cornerSizeCurrent.X;
            var afterCornerY = ActualLocation.Y + cornerSizeCurrent.Y;

            drawingInterface.DrawRectangle(afterCornerX,                                ActualLocation.Y,                           curSize.X - (cornerSizeCurrent.X*2),        scale, DrawLayer,LineColor * Alpha);
            drawingInterface.DrawRectangle(afterCornerX,                                ActualLocation.Y + curSize.Y- scale,        curSize.X - (cornerSizeCurrent.X*2),        scale, DrawLayer, LineColor * Alpha);
            drawingInterface.DrawRectangle(ActualLocation.X,                            afterCornerY,                               scale,                                      curSize.Y - (cornerSizeCurrent.Y*2), DrawLayer, LineColor * Alpha);
            drawingInterface.DrawRectangle(ActualLocation.X + curSize.X - scale,        afterCornerY,                               scale, curSize.Y - (cornerSizeCurrent.Y*2), DrawLayer, LineColor * Alpha);

            drawingInterface.DrawRectangle(afterCornerX,                                ActualLocation.Y + scale,                   curSize.X - (cornerSizeCurrent.X*2),        scale, DrawLayer, BackgroundColor * Alpha);
            drawingInterface.DrawRectangle(afterCornerX,                                ActualLocation.Y + curSize.Y - (scale*2),   curSize.X - (cornerSizeCurrent.X*2),        scale, DrawLayer, BackgroundColor * Alpha);
            drawingInterface.DrawRectangle(ActualLocation.X + scale,                    afterCornerY,                               scale,                                      curSize.Y - (cornerSizeCurrent.Y*2), DrawLayer, BackgroundColor * Alpha);
            drawingInterface.DrawRectangle(ActualLocation.X + curSize.X - (scale*2),    afterCornerY,                               scale,                                      curSize.Y - (cornerSizeCurrent.Y*2), DrawLayer, BackgroundColor * Alpha);
        }


    }
}
