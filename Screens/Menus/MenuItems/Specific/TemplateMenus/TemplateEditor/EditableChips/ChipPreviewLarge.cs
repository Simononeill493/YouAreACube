using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipPreviewLarge : SpriteMenuItem
    {
        public ChipData Chip;

        private TextMenuItem _title;

        public ChipPreviewLarge(IHasDrawLayer parent, ChipData data) : base(parent, "BlueChipFull")
        {
            Chip = data;
            _title = new TextMenuItem(this);
            _title.Color = Color.White;
            _title.Text = Chip.Name;

            _title.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_title);

            _addSections(Chip);
        }

        private void _addSections(ChipData chip)
        {
            var size = this.GetBaseSize();

            for(int i=1;i<chip.NumInputs+1;i++)
            {
                var section = new ChipPreviewLargeMiddleSection(ManualDrawLayer.Create(DrawLayer - (DrawLayers.MinLayerDistance * i)), chip.GetInputType(i));
                if (i == chip.NumInputs) { section.SpriteName = "BlueChipFullEnd"; }

                section.SetLocationConfig(ActualLocation.X, ActualLocation.Y + (size.Y*i) - 1, CoordinateMode.ParentPixelOffset, false);

                AddChild(section);
            }

            _updateChildDimensions();
        }

        public Point GetFullBaseSize()
        {
            var topSectionSize = GetBaseSize();
            return new Point(topSectionSize.X, topSectionSize.Y + (topSectionSize.Y * Chip.NumInputs));
        }
        public Point GetFullSize() => GetFullBaseSize() * Scale;
    }
}
