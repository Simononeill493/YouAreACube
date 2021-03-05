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
        private List<ChipPreviewLargeMiddleSection> _middleSections;

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
            _middleSections = new List<ChipPreviewLargeMiddleSection>();
            var size = this.GetBaseSize();

            for(int i=1;i<chip.NumInputs+1;i++)
            {
                var section = new ChipPreviewLargeMiddleSection(ManualDrawLayer.Create(DrawLayer - (DrawLayers.MinLayerDistance * i)), chip.GetInputType(i));
                if (i == chip.NumInputs) { section.SpriteName = "BlueChipFullEnd"; }

                section.SetLocationConfig(ActualLocation.X, ActualLocation.Y + (size.Y*i) - 1, CoordinateMode.ParentPixelOffset, false);

                _middleSections.Add(section);
                AddChild(section);
            }

            _updateChildDimensions();
        }

        public bool IsMouseOverAnySection()
        {
            if (MouseHovering)
            {
                return true;
            }

            foreach(var section in _middleSections)
            {
                if(section.MouseHovering)
                {
                    return true;
                }
            }

            return false;
        }


        public Point GetFullBaseSize()
        {
            var topSectionSize = GetBaseSize();
            return new Point(topSectionSize.X, topSectionSize.Y + (topSectionSize.Y * Chip.NumInputs));
        }
        public Point GetFullSize() => GetFullBaseSize() * Scale;
    }
}
