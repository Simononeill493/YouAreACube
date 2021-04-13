using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class DialogBoxMenuItem : TextBoxMenuItem
    {
        public DialogBoxMenuItem(IHasDrawLayer parentDrawLayer, string text) : base(parentDrawLayer,text)
        {
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            this.FillRestOfScreen(drawingInterface, DrawLayer + DrawLayers.MinLayerDistance, new Color(0,0,0,192), 0);
        }

    }
}
