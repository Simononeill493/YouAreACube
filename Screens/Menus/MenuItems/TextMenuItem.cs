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
            Dimensions.IsCentered = true;
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            if(Text!=null)
            {
                drawingInterface.DrawText(Text, X, Y, Dimensions.Scale-1, layer: DrawLayers.MenuTextLayer, centered: Dimensions.IsCentered);
            }

            base.Draw(drawingInterface);
        }

        public override bool IsMouseOver(UserInput input)
        {
            return false;
        }
    }
}
