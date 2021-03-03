using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateEditMenu : SpriteMenuItem
    {
        private Kernel _kernel;
        private BlockTemplate _template;

        public TemplateEditMenu(IHasDrawLayer parentDrawLayer,Kernel kernel, BlockTemplate template) : base(parentDrawLayer, "EditPaneWindow")
        {
            _kernel = kernel;
            _template = template;

            var chipEditPane = new ChipEditPane(this);
            var searchPane = new ChipSearchPane(this,chipEditPane.TryCreateChip);
            chipEditPane.Trash = searchPane;

            chipEditPane.SetLocationConfig(4, 4, CoordinateMode.ParentPixelOffset, false);
            searchPane.SetLocationConfig(84, 50, CoordinateMode.ParentPercentageOffset, true);

            AddChild(chipEditPane);
            AddChild(searchPane);
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            this.FillRestOfScreen(drawingInterface, DrawLayer + DrawLayers.MinLayerDistance, Color.Black, 1);
        }
    }
}
