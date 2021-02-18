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
        public TemplateEditScreen(Action<ScreenType> switchScreen,GameScreen gameScreen,BlockTemplate template) : base(ScreenType.TemplateEdit, switchScreen,gameScreen)
        {
            var templateEditor = new TemplateEditMenu(_gameScreen.Game.Kernel, template);
            templateEditor.SetLocationConfig(50, 50, CoordinateMode.Relative, centered: true);
            _addMenuItem(templateEditor);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if (input.IsKeyJustPressed(Keys.Escape))
            {
                SwitchScreen(ScreenType.TemplateExplorer);
            }
        }
    }
}
