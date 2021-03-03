﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipPreviewLarge : DraggableMenuItem
    {
        public ChipData Chip;
        private TextMenuItem _text;
           
        public ChipPreviewLarge(IHasDrawLayer parent, ChipData chip) : base(parent, "BlueChipFull")
        {
            Chip = chip;

            _text = new TextMenuItem(this);
            _text.Color = Color.White;
            _text.Text = chip.Name;

            _text.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_text);

            _addSections(chip);
        }

        private void _addSections(ChipData chip)
        {
            var size = this.GetBaseSize();

            for(int i=1;i<chip.NumInputs+1;i++)
            {
                var section = new ChipPreviewLargeMiddleSection(ManualDrawLayer.Create(DrawLayer - (DrawLayers.MinLayerDistance * i)), chip.GetInputType(i));
                if (i == chip.NumInputs) { section.SpriteName = "BlueChipFullEnd"; }

                section.SetLocationConfig(ActualLocation.X, ActualLocation.Y + (size.Y*i) - 1, CoordinateMode.ParentPixelOffset, false);

                this.SetDraggableFrom(section);
                AddChild(section);
            }

            _updateChildDimensions();
        }

        public Point GetFullSize()
        {
            var topSectionSize = GetCurrentSize();
            return new Point(topSectionSize.X, topSectionSize.Y + (topSectionSize.Y * Chip.NumInputs));
        }
    }
}
