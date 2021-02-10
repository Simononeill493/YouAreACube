using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateExplorerScreen : MenuScreen
    {
        public GameScreen _game;

        public TemplateExplorerScreen(Action<ScreenType> switchScreen, GameScreen game) : base(switchScreen)
        {
            _game = game;

            var newGameButton = new MenuItem()
            {
                SpriteName = "EmptyMenuRectangleMedium",
                XPercentage = 50,
                YPercentage = 25,
                Scale = 3,
            };

            MenuItems.Add(newGameButton);
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            base.Draw(drawingInterface);
            _game.Draw(drawingInterface);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if (input.IsKeyJustPressed(Keys.Tab))
            {
                SwitchScreen(ScreenType.Game);
            }
        }
    }
}
