using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateEditScreen : InGameMenuScreen
    {
        private TemplateEditMenu _templateEditMenu;

        public TemplateEditScreen(Action<ScreenType> switchScreen,GameScreen gameScreen,CubeTemplate template) : base(ScreenType.TemplateEdit, switchScreen,gameScreen)
        {
            _templateEditMenu = new TemplateEditMenu(this, _gameScreen._gameHolder.CurrentGame.Kernel, template, _returnToTemplateExplorer);
            _templateEditMenu.SetLocationConfig(50, 50, CoordinateMode.ParentPercentage, centered: true);
            _addMenuItem(_templateEditMenu);


            AddKeyJustReleasedEvent(Keys.Escape, (i) => _templateEditMenu.OpenQuitDialog());
        }

        private void _returnToTemplateExplorer()
        {
            SwitchScreen(ScreenType.TemplateExplorer);
        }


    }
}
