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
        public List<EditableChipset> AllChipsets;
        public float ChipScaleMultiplier = 1.0f;
        public ChipEditPane(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "ChipEditPane")
        {
            DrawLayer = DrawLayers.MenuBehindLayer;

            AllChipsets = new List<EditableChipset>();

            var size = GetBaseSize();

            var plusButton = new SpriteMenuItem(this, "PlusButton");
            plusButton.SetLocationConfig(size.X - 9, 0, CoordinateMode.ParentPixelOffset, false);
            plusButton.UpdateDrawLayerCascade(DrawLayer - (DrawLayers.MinLayerDistance*10));
            plusButton.OnMouseReleased += (i) => _multiplyChipsetScales(2);
            AddChild(plusButton);

            var minusButton = new SpriteMenuItem(this, "MinusButton");
            minusButton.SetLocationConfig(size.X - 17, 0, CoordinateMode.ParentPixelOffset, false);
            minusButton.UpdateDrawLayerCascade(DrawLayer - (DrawLayers.MinLayerDistance * 10));
            minusButton.OnMouseReleased += (i) => _multiplyChipsetScales(0.5f);
            AddChild(minusButton);
        }



        public void TryCreateChipset(ChipPreviewSmall preview, UserInput input)
        {
            if (MenuScreen.UserDragging) { return; }

            var chipset = EditableChipset.CreateAtMouse(input, this);

            chipset.AddChip(preview.Chip,ChipScaleMultiplier);

            AllChipsets.Add(chipset);
            AddChildAfterUpdate(chipset);
        }
        public void DeleteChipset(EditableChipset chip)
        {
            AllChipsets.Remove(chip);
            RemoveChildAfterUpdate(chip);
        }
        public void ChipsetReleased(EditableChipset chip, UserInput input)
        {
            if (Trash.IsMouseOver(input))
            {
                DeleteChipset(chip);
            }
            else
            {
                _attachChipsetToPane(chip, GetCurrentSize());
                _setChipsetVisiblity(chip);
            }
        }

        protected override void _updateChildDimensions()
        {
            var newChipScale = MenuItem.GenerateScaleFromMultiplier(ChipScaleMultiplier);
            if (newChipScale < 2)
            {
                var toTwoMultiplier = 2.0f / newChipScale;
                _multiplyChipsetScales(toTwoMultiplier);
            }

            base._updateChildDimensions();
            _setChipsetVisibilities();
        }

        private void _multiplyChipsetScales(float multiplier)
        {
            var newChipScale = MenuItem.GenerateScaleFromMultiplier(ChipScaleMultiplier * multiplier);
            if (newChipScale < 2) { return; }

            ChipScaleMultiplier *= multiplier;
            var size = GetCurrentSize();

            foreach (var chip in AllChipsets)
            {
                chip.MultiplyScaleCascade(multiplier);
            }

            _updateChildDimensions();
        }
        private void _attachChipsetToPane(EditableChipset chip, Point planeSize)
        {
            var displacement = this.GetLocationOffset(chip.ActualLocation);

            chip.SetLocationConfig((displacement / chip.Scale), CoordinateMode.ParentPixelOffset, false);
            chip.UpdateDimensionsCascade(ActualLocation, planeSize);
        }
        private void _setChipsetVisibilities()
        {
            foreach (var chip in AllChipsets)
            {
                _setChipsetVisiblity(chip);
            }
        }
        private void _setChipsetVisiblity(EditableChipset chip)
        {
            var chipLoc = chip.ActualLocation;
            var fullSize = chip.GetFullSize();

            var trueRect = new Rectangle(chipLoc.X,chipLoc.Y,fullSize.X,fullSize.Y);
            var isIntersected = this.IsIntersectedWith(trueRect);
            chip.Visible = isIntersected;
        }
    }
}
