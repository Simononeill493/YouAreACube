using Microsoft.Xna.Framework;
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
            DrawLayer = DrawLayers.MenuBehindLayer;

            AllChips = new List<ChipPreviewLarge>();

            var size = GetBaseSize();

            var plusButton = new SpriteMenuItem(this, "PlusButton");
            plusButton.SetLocationConfig(size.X - 9, 0, CoordinateMode.ParentPixelOffset, false);
            plusButton.UpdateDrawLayerCascade(DrawLayer - (DrawLayers.MinLayerDistance*10));
            plusButton.OnClick += (i) => _multiplyChipsScale(2);
            AddChild(plusButton);

            var minusButton = new SpriteMenuItem(this, "MinusButton");
            minusButton.SetLocationConfig(size.X - 17, 0, CoordinateMode.ParentPixelOffset, false);
            minusButton.UpdateDrawLayerCascade(DrawLayer - (DrawLayers.MinLayerDistance * 10));
            minusButton.OnClick += (i) => _multiplyChipsScale(0.5f);
            AddChild(minusButton);
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

        public void DeleteChip(ChipPreviewLarge chip)
        {
            AllChips.Remove(chip);
            RemoveChildAfterUpdate(chip);
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
                _setChipVisiblity(chip);
            }
        }

        protected override void _updateChildDimensions()
        {
            var newChipScale = MenuItem.GenerateScaleFromMultiplier(_currentChipScaleMultiplier);
            if (newChipScale < 2)
            {
                var toTwoMultiplier = 2.0f / newChipScale;
                _multiplyChipsScale(toTwoMultiplier);
            }

            base._updateChildDimensions();
            _setChipVisibilities();
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
        private void _attachChipToPane(ChipPreviewLarge chip, Point planeSize)
        {
            var displacement = this.GetLocationOffset(chip.ActualLocation);

            chip.SetLocationConfig((displacement / chip.Scale), CoordinateMode.ParentPixelOffset, false);
            chip.UpdateDimensionsCascade(ActualLocation, planeSize);
        }
        private void _setChipVisibilities()
        {
            foreach (var chip in AllChips)
            {
                _setChipVisiblity(chip);
            }
        }
        private void _setChipVisiblity(ChipPreviewLarge chip)
        {
            var chipLoc = chip.ActualLocation;
            var fullSize = chip.GetFullSize();

            var trueRect = new Rectangle(chipLoc.X,chipLoc.Y,fullSize.X,fullSize.Y);
            var isIntersected = this.IsIntersectedWith(trueRect);
            chip.Visible = isIntersected;
        }
    }
}
