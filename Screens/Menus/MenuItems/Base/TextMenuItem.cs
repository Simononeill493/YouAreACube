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

        public TextMenuItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer) { }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            if(Text!=null)
            {
                drawingInterface.DrawText(Text, ActualLocation.X, ActualLocation.Y, Scale, layer: DrawLayer);
            }
        }

        public override bool IsMouseOver(UserInput input)
        {
            return false;
        }

        public override Point GetSize()
        {
            var size = SpriteManager.GetTextSize(Text);
            return size * Scale;
        }
    }
}
