using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    abstract class VisualScreenItem : ScreenItem
    {
        public Color CurrentColor => (MouseHovering) ? (HighlightColor): DefaultColor;
        public Color DefaultColor = GlobalDefaultColor;
        public Color HighlightColor = GlobalDefaultColor;

        public void SetConstantColor(Color color)
        { 
            DefaultColor = color;
            HighlightColor = color;
        }

        public void ResetColor()
        {
            DefaultColor = GlobalDefaultColor;
        }

        public static Color GlobalDefaultColor = Color.White;


        public virtual float Alpha { get; set; } = 1.0f;

        public bool FlipHorizontal;
        public bool FlipVertical;

        public VisualScreenItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer) { }



    }
}
