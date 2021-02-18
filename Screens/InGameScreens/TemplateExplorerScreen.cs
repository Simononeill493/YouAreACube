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
        private GameScreen _game;

        public TemplateExplorerScreen(Action<ScreenType> switchScreen, Action<BlockTemplate> openTemplateForEditing,GameScreen game) : base(ScreenType.TemplateExplorer, switchScreen)
        {
            _game = game;

            var templateMenu = new TemplateExplorerMenu(game.Game.Kernel, openTemplateForEditing);
            _addMenuItem(templateMenu);
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            base.Draw(drawingInterface);
            _game.Draw(drawingInterface);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if (input.IsKeyJustPressed(Keys.Tab) || input.IsKeyJustPressed(Keys.Escape))
            {
                SwitchScreen(ScreenType.Game);
            }
        }
    }
}
