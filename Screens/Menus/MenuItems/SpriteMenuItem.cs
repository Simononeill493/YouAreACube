using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class SpriteMenuItem : MenuItem
    {
        public string SpriteName;
        public string HighlightedSpriteName;

        public SpriteMenuItem()
        {
            IsCentered = true;
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            if (_mouseHovering & HighlightedSpriteName != null)
            {
                drawingInterface.DrawSprite(HighlightedSpriteName, LocationOnScreen.X, LocationOnScreen.Y, Config.MenuItemScale, layer: DrawLayers.MenuItemLayer, centered: IsCentered);
            }
            else
            {
                drawingInterface.DrawSprite(SpriteName, LocationOnScreen.X, LocationOnScreen.Y, Config.MenuItemScale, layer: DrawLayers.MenuItemLayer, centered: IsCentered);
            }

            base.Draw(drawingInterface);
        }

        public override bool IsMouseOver(UserInput input)
        {
            var rect = DrawUtils.GetMenuItemRectangle(this, Config.MenuItemScale);
            return rect.Contains(input.MouseX, input.MouseY);
        }

    }
}
