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
        public string SpriteName { get { return _spriteName; } 
            set 
            { 
                _spriteName = value;
                _spriteSize = SpriteManager.GetSpriteSize(SpriteName);
            } 
        }
        private string _spriteName;

        public string HighlightedSpriteName;

        public Color ColorMask = Color.White;
        public bool FlipHorizontal;
        public bool FlipVertical;

        private string _currentSprite => (MouseHovering & (HighlightedSpriteName != null)) ? HighlightedSpriteName : SpriteName;

        public SpriteMenuItem(IHasDrawLayer parentDrawLayer,string spriteName) : base(parentDrawLayer)
        {
            SpriteName = spriteName;
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawSprite(_currentSprite, ActualLocation.X, ActualLocation.Y, Scale, DrawLayer, ColorMask, flipHorizontal: FlipHorizontal);
        }

        public override IntPoint GetBaseSize() => _spriteSize;
        private IntPoint _spriteSize;
    }
}
