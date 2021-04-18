using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateExplorerScreen : InGameMenuScreen
    {
        public TemplateExplorerScreen(Action<ScreenType> switchScreen, Action<BlockTemplate> openTemplateForEditing,GameScreen gameScreen) : base(ScreenType.TemplateExplorer, switchScreen,gameScreen)
        {
            var templateMenu = new TemplateExplorerMenu(this,_gameScreen.Game.Kernel, openTemplateForEditing);
            templateMenu.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, centered: true);

            _addMenuItem(templateMenu);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if (input.IsKeyJustPressed(Keys.Escape))
            {
                _returnToGame();
            }
        }
    }
}
