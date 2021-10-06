using System;
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

            var dialogue = new TextDialogueBox(this, new IntPoint(140, 50), 63);

            dialogue.SetLocationConfig(-13, -29, CoordinateMode.ParentPixel, true);
            dialogue.MultiplyScale(0.75f);

            //dialogue.AddAnimation(new FadeInAnimation(Triggers.Timed(180), Tickers.Constant, 0.08f));
            //dialogue.AddAnimation(new MovementAnimation(Triggers.Timed(180), Tickers.Cyclic(1), IntPoint.Down, 3));
            buddy.AddChild(dialogue);

            var buddyHolder = new ContainerScreenItem(this);
            buddyHolder.SetLocationConfig(65, 100, CoordinateMode.ParentPercentage, false);
            buddyHolder.AddChild(buddy);
            _addMenuItem(buddyHolder);

            dialogue.SetScenario(TestScenarioGenerator.GenerateTestScenario());
        }
    }

}
