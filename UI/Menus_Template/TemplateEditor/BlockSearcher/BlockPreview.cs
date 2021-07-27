﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace IAmACube
{
    class BlockPreview : SpriteMenuItem
    {
        public BlockData Block;
        private TextMenuItem _text;

        public BlockPreview(IHasDrawLayer parent, BlockData chip) : base(parent, "ChipSmall")
        {
            Block = chip;
            ColorMask = Block.ChipDataType.GetColor();

            _text = new TextMenuItem(this);
            _text.Color = Color.White;
            _text.MultiplyScale(0.5f);
            _text.Text = chip.Name;

            _text.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_text);
        }
    }
}