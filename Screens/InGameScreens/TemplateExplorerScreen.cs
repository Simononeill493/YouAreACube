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

            var newGameButton = new ShapeMenuItem()
            {
                Color = Microsoft.Xna.Framework.Color.Gray,
                Width = 128,
                Height = 128
            };

            newGameButton.SetPositioningConfig(50, 25, CoordinateMode.Relative);

            AddMenuItem(newGameButton);
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
