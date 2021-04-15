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
        private TemplateEditMenu _templateEditor;

        public TemplateEditScreen(Action<ScreenType> switchScreen,GameScreen gameScreen,BlockTemplate template) : base(ScreenType.TemplateEdit, switchScreen,gameScreen)
        {
            _templateEditor = new TemplateEditMenu(this, _gameScreen.Game.Kernel, template, () => SwitchScreen(ScreenType.TemplateExplorer));
            _templateEditor.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, centered: true);
            _addMenuItem(_templateEditor);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if (input.IsKeyJustReleased(Keys.Escape))
            {
                _templateEditor.OpenQuitDialog();
            }
        }
    }
}
