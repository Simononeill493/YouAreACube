using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class PreDemoScreen :MenuScreen
    {
        public const string DemoBuddy = "DemoBuddy";

        public PreDemoScreen(Action<ScreenType> switchScreen) : base(ScreenType.PreDemo, switchScreen)
        {
            _manualResizeEnabled = false;

            var buddy = new SpriteScreenItem(this, DemoBuddy);
            buddy.SetLocationConfig(0, 0, CoordinateMode.ParentPixel, false);
            buddy.MultiplyScale(4.0f);

            var moveAnimation = new MoveToPixelOffsetAnimation(Triggers.Timed(0),Tickers.Cyclic(0), new IntPoint(0,-10));
            buddy.AddAnimation(moveAnimation);

            var buddyHolder = new ContainerScreenItem(this);
            buddyHolder.SetLocationConfig(75, 100, CoordinateMode.ParentPercentage, false);
            buddyHolder.AddChild(buddy);
            _addMenuItem(buddyHolder);
        }
    }
}
