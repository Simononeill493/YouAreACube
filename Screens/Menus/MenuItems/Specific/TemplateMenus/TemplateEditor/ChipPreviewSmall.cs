﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace IAmACube
{
    class ChipPreviewSmall : SpriteMenuItem
    {
        public ChipData Chip;
        private TextMenuItem _text;

        public ChipPreviewSmall(IHasDrawLayer parent, ChipData chip) : base(parent, "BlueChipSmall")
        {
            Chip = chip;

            _text = new TextMenuItem(this);
            _text.Color = Color.White;
            _text.HalfScaled = true;
            _text.Text = chip.Name;

            _text.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_text);
        }
    }
}
