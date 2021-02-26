using Microsoft.Xna.Framework;
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
        private int _addedSections;

        public ChipPreviewLarge(IHasDrawLayer parent, ChipData chip) : base(parent, "BlueChipFull")
        {
            Chip = chip;

            _text = new TextMenuItem(this);
            _text.Color = Color.White;
            _text.HalfScaled = true;
            _text.Text = chip.Name;

            _text.SetLocationConfig(10, 40, CoordinateMode.ParentPercentageOffset, false);
            AddChild(_text);
        }
    }
}
