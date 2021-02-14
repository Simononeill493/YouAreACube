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
            IsCentered = true;
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            if(Text!=null)
            {
                drawingInterface.DrawText(Text, LocationOnScreen.X, LocationOnScreen.Y, Config.MenuItemTextScale, layer: DrawLayers.MenuTextLayer, centered: IsCentered);
            }

            base.Draw(drawingInterface);
        }
        public override bool IsMouseOver(UserInput input)
        {
            return false;
        }
    }
}
