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
        public TemplateExplorerScreen(Action<ScreenType> switchScreen, Action<CubeTemplate> openTemplateForEditing, GameScreen gameScreen) : base(ScreenType.TemplateExplorer, switchScreen,gameScreen)
        {
            var templateMenu = new TemplateExplorerMenuMain(this,_gameScreen.Game.Kernel, openTemplateForEditing);
            //templateMenu.MakeBoxes();
            templateMenu.SetLocationConfig(50, 50, CoordinateMode.ParentPercentage, centered: true);

            _addMenuItem(templateMenu);

            AddKeyJustReleasedEvent(Keys.Escape, (i) => _returnToGame());
        }
    }
}
