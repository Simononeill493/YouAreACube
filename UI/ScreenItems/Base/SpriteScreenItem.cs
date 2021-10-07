using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace IAmACube
{
    class SpriteScreenItem : VisualScreenItem
    {
        private string _currentSprite => (MouseHovering & (HighlightedSpriteName != null)) ? HighlightedSpriteName : SpriteName;

        public string SpriteName { get { return _spriteName; } 
            set 
            { 
                _spriteName = value;
                _spriteSize = SpriteManager.GetSpriteSize(SpriteName);
            } 
        }
        private string _spriteName;

        public string HighlightedSpriteName;

        public SpriteScreenItem(IHasDrawLayer parentDrawLayer,string spriteName) : base(parentDrawLayer)
        {
            SpriteName = spriteName;
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            drawingInterface.DrawSprite(_currentSprite, ActualLocation.X, ActualLocation.Y, Scale, DrawLayer, CurrentColor * Alpha, flipHorizontal: FlipHorizontal, flipVertical: FlipVertical);
        }

        public override IntPoint GetBaseSize() => _spriteSize;
        private IntPoint _spriteSize;
    }
}
