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

        private string _activeSpriteName => (_mouseHovering & HighlightedSpriteName != null) ? HighlightedSpriteName : SpriteName;

        public SpriteMenuItem()
        {
            Dimensions.IsCentered = true;
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawSprite(_activeSpriteName, X, Y, Config.MenuItemScale, layer: DrawLayers.MenuItemLayer, centered: Dimensions.IsCentered);
            base.Draw(drawingInterface);
        }
        public override bool IsMouseOver(UserInput input)
        {
            var rect = DrawUtils.GetMenuItemRectangle(this.SpriteName,this.Dimensions, Config.MenuItemScale);
            return rect.Contains(input.MouseX, input.MouseY);
        }
    }
}
