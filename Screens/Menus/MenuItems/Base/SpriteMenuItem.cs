using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class SpriteMenuItem : MenuItem
    {
        public string SpriteName;
        public string HighlightedSpriteName;

        private string _activeSpriteName => (_mouseHovering & HighlightedSpriteName != null) ? HighlightedSpriteName : SpriteName;

        public SpriteMenuItem(IHasDrawLayer parentDrawLayer,string spriteName) : base(parentDrawLayer)
        {
            SpriteName = spriteName;
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawSprite(_activeSpriteName, ActualLocation.X, ActualLocation.Y, Scale, layer: DrawLayer);
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
