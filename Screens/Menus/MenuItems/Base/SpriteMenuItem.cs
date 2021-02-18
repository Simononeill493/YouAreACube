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

        public SpriteMenuItem(string spriteName)
        {
            SpriteName = spriteName;
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawSprite(_activeSpriteName, ActualLocation.X, ActualLocation.Y, Scale, layer: DrawLayer);
            base.Draw(drawingInterface);
        }

        public override bool IsMouseOver(UserInput input)
        {
            var rect = DrawUtils.GetSpriteDimensions(this.SpriteName, ActualLocation.X, ActualLocation.Y, Scale);
            return rect.Contains(input.MouseX, input.MouseY);
        }

        public override Point GetSize()
        {
            var size = SpriteManager.GetSpriteSize(SpriteName);
            return (size) * Scale;
        }
    }
}
