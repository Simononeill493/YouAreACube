using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipEditPane : SpriteMenuItem
    {
        public MenuItem Trash;
        public List<ChipPreviewLarge> AllChips;

        private float _currentChipScaleMultiplier = 1.0f;

        public ChipEditPane(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "ChipEditPane")
        {
            AllChips = new List<ChipPreviewLarge>();

            var size = GetBaseSize();

            var plusButton = new SpriteMenuItem(this, "PlusButton");
            plusButton.SetLocationConfig(size.X - 9, 0, CoordinateMode.ParentPixelOffset, false);
            plusButton.OnClick += (i) => _multiplyChipsScale(2);
            AddChild(plusButton);

            var minusButton = new SpriteMenuItem(this, "MinusButton");
            minusButton.SetLocationConfig(size.X - 17, 0, CoordinateMode.ParentPixelOffset, false);
            minusButton.OnClick += (i) => _multiplyChipsScale(0.5f);
            AddChild(minusButton);
        }
        
        private void _multiplyChipsScale(float multiplier)
        {
            var newChipScale = MenuItem.GenerateScaleFromMultiplier(_currentChipScaleMultiplier * multiplier);
            if (newChipScale < 2) { return; }

            _currentChipScaleMultiplier *= multiplier;
            var size = GetCurrentSize();

            foreach (var chip in AllChips)
            {
                chip.MultiplyScaleCascade(multiplier);
                chip.UpdateDimensions(ActualLocation, size);
            }

            _updateChildDimensions();
        }

        public void TryCreateChip(ChipPreviewSmall preview, UserInput input)
        {
            if (MenuScreen.UserDragging) { return; }

            var chip = new ChipPreviewLarge(this, preview.Chip);
            chip.MultiplyScaleCascade(_currentChipScaleMultiplier);
            chip.SetLocationConfig(input.MousePos, CoordinateMode.Absolute, true);
            chip.UpdateDimensions(ActualLocation,GetCurrentSize());
            chip.OnEndDrag += (i) => ChipReleased(chip, i);

            if (!chip.TryBeginDrag(input, chip.GetCurrentSize() / 2))
            {
                return;
            }

            AllChips.Add(chip);
            AddChildAfterUpdate(chip);
        }

        public override void UpdateDimensions(Point parentlocation, Point parentSize)
        {
            base.UpdateDimensions(parentlocation, parentSize);

            var newChipScale = MenuItem.GenerateScaleFromMultiplier(_currentChipScaleMultiplier);
            if (newChipScale < 2) 
            {
                var toTwoMultiplier = 2.0f / newChipScale;
                _multiplyChipsScale(toTwoMultiplier);
            }
        }


        public void ChipReleased(ChipPreviewLarge chip, UserInput input)
        {
            if (Trash.IsMouseOver(input))
            {
                DeleteChip(chip);
            }
            else
            {
                _attachChipToPane(chip, GetCurrentSize());
            }
        }

        public void DeleteChip(ChipPreviewLarge chip)
        {
            AllChips.Remove(chip);
            RemoveChildAfterUpdate(chip);
        }


        private void _attachChipToPane(ChipPreviewLarge chip, Point planeSize)
        {
            var displacement = this.GetLocationOffset(chip.ActualLocation);

            chip.SetLocationConfig((displacement / chip.Scale), CoordinateMode.ParentPixelOffset, false);
            chip.UpdateDimensionsCascade(ActualLocation, planeSize);
        }
    }
}
