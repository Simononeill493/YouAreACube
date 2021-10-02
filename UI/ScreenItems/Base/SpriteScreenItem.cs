﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace IAmACube
{
    public class SpriteScreenItem : ScreenItem
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
        public float Alpha = 1.0f;

        public bool FlipHorizontal;
        public bool FlipVertical;

        private string _currentSprite => (MouseHovering & (HighlightedSpriteName != null)) ? HighlightedSpriteName : SpriteName;

        public SpriteScreenItem(IHasDrawLayer parentDrawLayer,string spriteName) : base(parentDrawLayer)
        {
            SpriteName = spriteName;
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            drawingInterface.DrawSprite(_currentSprite, ActualLocation.X, ActualLocation.Y, Scale, DrawLayer, ColorMask * Alpha, flipHorizontal: FlipHorizontal, flipVertical: FlipVertical);
        }

        public override IntPoint GetBaseSize() => _spriteSize;
        private IntPoint _spriteSize;
    }
}
