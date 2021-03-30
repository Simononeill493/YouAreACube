using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace IAmACube
{
    class ChipPreview : SpriteMenuItem
    {
        public ChipData Chip;
        private TextMenuItem _text;

        public ChipPreview(IHasDrawLayer parent, ChipData chip) : base(parent, "ChipSmall")
        {
            Chip = chip;
            ColorMask = Chip.ChipDataType.GetColor();

            _text = new TextMenuItem(this);
            _text.Color = Color.White;
            _text.MultiplyScale(0.5f);
            _text.Text = chip.Name;

            _text.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_text);
        }

        public ChipTop GenerateChip()
        {
            var initialDrawLayer = ManualDrawLayer.Zero;

            if (Chip.Name.Equals("If"))
            {
                return new ChipTopIfElse(initialDrawLayer, this.Chip);
            }
            else
            {
                return new ChipTopStandard(initialDrawLayer, this.Chip);
            }
        }

    }
}
