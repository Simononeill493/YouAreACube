using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class TextMenuItem : MenuItem
    {
        public string Text;
        public Color Color = Config.DefaultTextColor;

        public TextMenuItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer) { }
        public TextMenuItem(IHasDrawLayer parentDrawLayer,string initial) : base(parentDrawLayer) { Text = initial; }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            if(Text!=null)
            {
                drawingInterface.DrawText(Text, ActualLocation.X, ActualLocation.Y, Scale, DrawLayer, Color);
            }
        }

        public override bool IsMouseOver(UserInput input) => false;
        public override Point GetBaseSize() => SpriteManager.GetTextSize(Text);
    }
}
