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

            var dialogue = new DialogueBox(this, new IntPoint(80, 30),60);
            dialogue.SetLocationConfig(-13, -29, CoordinateMode.ParentPixel, true);
            dialogue.MultiplyScale(0.75f);
            dialogue.AddAnimation(new FadeInAnimation(Triggers.Timed(180), Tickers.Constant,0.08f));
            dialogue.AddAnimation(new MovementAnimation(Triggers.Timed(180), Tickers.Cyclic(1),IntPoint.Down,3));
            buddy.AddChild(dialogue);

            var buddyHolder = new ContainerScreenItem(this);
            buddyHolder.SetLocationConfig(65, 100, CoordinateMode.ParentPercentage, false);
            buddyHolder.AddChild(buddy);
            _addMenuItem(buddyHolder);
        }
    }

    class DialogueBox : CorneredBoxMenuItem
    {
        public const string PointSprite = "DialogueBoxPoint";

        public override float Alpha { get => base.Alpha; set { base.Alpha = value; _point.Alpha = value; } }
        private SpriteScreenItem _point;

        private List<TextScreenItem> _textItems;
        private string _text1 = "";
        private string _text2 = "";

        public DialogueBox(IHasDrawLayer parent,IntPoint size,int pointOffset) :base(parent,size)
        {
            _point = new SpriteScreenItem(ManualDrawLayer.InFrontOf(this,5), PointSprite);
            _point.SetLocationConfig(pointOffset, size.Y-1, CoordinateMode.ParentPixel, false);
            AddChild(_point);

            _textItems = new List<TextScreenItem>();
            var text1 = new TextScreenItem(this, () => _text1);
            var text2 = new TextScreenItem(this, () => _text2);

            AddChild(text1);
            AddChild(text2);

            _textItems.Add(text1);
            _textItems.Add(text2);

            Alpha = 0;
        }
    }

}
