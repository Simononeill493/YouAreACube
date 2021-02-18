using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TextMenuItem : MenuItem
    {
        public string Text;

        public TextMenuItem()
        {
            DrawLayer = DrawLayers.MenuContentsBackLayer;
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            if(Text!=null)
            {
                drawingInterface.DrawText(Text, ActualLocation.X, ActualLocation.Y, Scale-1, layer: DrawLayer);
            }

            base.Draw(drawingInterface);
        }

        public override bool IsMouseOver(UserInput input)
        {
            return false;
        }

        public override Point GetSize()
        {
            var size = SpriteManager.GetTextSize(Text);
            return size * (Scale-1);
        }
    }
}
