using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace IAmACube
{
    public class SpriteMenuItem : MenuItem
    {
        public string SpriteName;
        public string HighlightedSpriteName;

        public Color ColorMask = Color.White;

        private string _currentSprite => (MouseHovering & (HighlightedSpriteName != null)) ? HighlightedSpriteName : SpriteName;

        public SpriteMenuItem(IHasDrawLayer parentDrawLayer,string spriteName) : base(parentDrawLayer)
        {
            SpriteName = spriteName;
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawSprite(_currentSprite, ActualLocation.X, ActualLocation.Y, Scale, DrawLayer, ColorMask, false);
        }

        public override Point GetBaseSize() => SpriteManager.GetSpriteSize(SpriteName);
    }
}
