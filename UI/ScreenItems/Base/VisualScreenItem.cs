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
        public Color Color = Color.White;

        public virtual float Alpha { get; set; } = 1.0f;

        public bool FlipHorizontal;
        public bool FlipVertical;

        public VisualScreenItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer) { }

    }
}
