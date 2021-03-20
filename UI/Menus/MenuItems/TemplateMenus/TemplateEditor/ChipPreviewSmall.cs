using System;
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
            _text.MultiplyScale(0.5f);
            _text.Text = chip.Name;

            _text.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_text);
        }

        public ChipPreviewLarge GenerateChip(float scale)
        {
            var chipHoverLayer = ManualDrawLayer.Create(DrawLayers.MenuBehindLayer-DrawLayers.MinLayerDistance);
            var chip = new ChipPreviewLarge(chipHoverLayer, this.Chip);
            chip.MultiplyScaleCascade(scale);

            return chip;
        }

    }
}
