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
        public GraphicalChipData Chip;
        private TextMenuItem _text;

        public ChipPreview(IHasDrawLayer parent, GraphicalChipData chip) : base(parent, "ChipSmall")
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
    }
}
