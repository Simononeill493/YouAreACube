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

        public override void Draw(DrawingInterface drawingInterface)
        {
            if(Text!=null)
            {
                drawingInterface.DrawText(Text, Location.X, Location.Y, Scale-1, layer: DrawLayers.MenuTextLayer);
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
