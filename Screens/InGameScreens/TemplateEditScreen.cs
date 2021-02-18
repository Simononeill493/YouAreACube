using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateEditScreen : MenuScreen
    {
        public TemplateEditScreen(Action<ScreenType> switchScreen,BlockTemplate template) : base(ScreenType.TemplateEdit, switchScreen)
        {
            //Background = template.Sprite;

            var sprite1 = new SpriteMenuItem("EmptyMenuRectangleFull");
            sprite1.SetLocation(50, 50, CoordinateMode.Absolute);
            _addMenuItem(sprite1);

            var sprite2 = new SpriteMenuItem(template.Sprite);
            sprite2.AttachLocationToParent(sprite1, new Point(10, 10), CoordinateMode.Absolute);
            sprite2.DrawLayer = sprite1.DrawLayer - 0.1f;
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if (input.IsKeyJustPressed(Keys.Tab))
            {
                SwitchScreen(ScreenType.Game);
            }
            if (input.IsKeyJustPressed(Keys.Escape))
            {
                SwitchScreen(ScreenType.TemplateExplorer);
            }
        }
    }
}
