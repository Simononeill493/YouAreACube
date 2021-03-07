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
        private const float MinimumChipScale = 2.0f;

        public ChipSearchPane SearchPane;
        public List<EditableChipset> Chipsets;
        public float ChipScaleMultiplier = 1.0f;
        public ChipEditPane(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "ChipEditPane")
        {
            DrawLayer = DrawLayers.MenuBehindLayer;

            Chipsets = new List<EditableChipset>();

            var size = GetBaseSize();

            var plusButton = new SpriteMenuItem(this, "PlusButton");
            plusButton.SetLocationConfig(size.X - 9, 0, CoordinateMode.ParentPixelOffset, false);
            plusButton.UpdateDrawLayerCascade(DrawLayer - (DrawLayers.MinLayerDistance*10));
            plusButton.OnMouseReleased += (i) => _multiplyChipScale(2);
            AddChild(plusButton);

            var minusButton = new SpriteMenuItem(this, "MinusButton");
            minusButton.SetLocationConfig(size.X - 17, 0, CoordinateMode.ParentPixelOffset, false);
            minusButton.UpdateDrawLayerCascade(DrawLayer - (DrawLayers.MinLayerDistance * 10));
            minusButton.OnMouseReleased += (i) => _multiplyChipScale(0.5f);
            AddChild(minusButton);
        }

        public void TryCreateChipsetFromSearchPane(ChipPreviewSmall preview, UserInput input)
        {
            if (MenuScreen.UserDragging) { return; }

            var chip = new ChipPreviewLarge(ManualDrawLayer.Create(DrawLayers.MenuHoverLayer), preview.Chip);
            chip.MultiplyScaleCascade(ChipScaleMultiplier);

            _createChipset(input, new List<ChipPreviewLarge>() { chip });
        }
        private void _createChipset(UserInput input, List<ChipPreviewLarge> chips)
        {
            var chipset = EditableChipset.CreateAtMouse(input, this);
            chipset.SetCreateNewChipsetCallback(_createChipset);
            chipset.AppendChips(chips, 0);
            chipset.TryStartDrag(input, chips.First().GetCurrentSize() / 2);

            foreach (var chip in chips)
            {
                chip.UpdateDrawLayerCascade(chipset.DrawLayer - DrawLayers.MinLayerDistance);
            }

            Chipsets.Add(chipset);
            AddChildAfterUpdate(chipset);
        }

        public void RemoveChipset(EditableChipset chipset)
        {
            Chipsets.Remove(chipset);
            RemoveChildAfterUpdate(chipset);
            chipset.Dispose();
        }
        
        public void ChipsetReleased(EditableChipset chipset, UserInput input)
        {
            if (SearchPane.IsMouseOver(input))
            {
                RemoveChipset(chipset);
            }
            else
            {
                var (chipDroppedOn,index) = _getFirstChipsetThatMouseIsOver(input,chipset);
                if(chipDroppedOn != null)
                {
                    RemoveChipset(chipset);
                    chipDroppedOn.AppendChips(chipset.Chips, index);
                }
                else
                {
                    _attachChipsetToPane(chipset, GetCurrentSize());
                    _setChipsetVisiblity(chipset);
                }
            }
        }

        protected override void _updateChildDimensions()
        {
            _pushChipScalingUpIfTooSmall();
            base._updateChildDimensions();
            _setChipsetVisibilities();
        }

        private (EditableChipset chipset, int chipIndex) _getFirstChipsetThatMouseIsOver(UserInput input, EditableChipset currentlyHovering)
        {
            foreach (var chipset in Chipsets)
            {
                if (chipset != currentlyHovering)
                {
                    var pos = chipset.GetChipIndexThatMouseIsOver(input);
                    if (pos != -1)
                    {
                        return (chipset, pos);
                    }
                }
            }

            return (null, -1);
        }

        private void _attachChipsetToPane(EditableChipset chip, Point planeSize)
        {
            var displacement = this.GetLocationOffset(chip.ActualLocation);

            chip.SetLocationConfig((displacement / chip.Scale), CoordinateMode.ParentPixelOffset, false);
            chip.UpdateDimensionsCascade(ActualLocation, planeSize);
        }

        private void _setChipsetVisibilities()
        {
            foreach (var chip in Chipsets)
            {
                _setChipsetVisiblity(chip);
            }
        }
        private void _setChipsetVisiblity(EditableChipset chip)
        {
            var isIntersected = this.IsIntersectedWith(chip.GetFullRect());
            chip.Visible = isIntersected;
        }

        private void _multiplyChipScale(float multiplier)
        {
            ChipScaleMultiplier *= multiplier;

            foreach (var chip in Chipsets)
            {
                chip.MultiplyScaleCascade(multiplier);
            }

            _updateChildDimensions();
        }
        private void _pushChipScalingUpIfTooSmall()
        {
            var currentChipScale = GenerateScaleFromMultiplier(ChipScaleMultiplier);
            if (currentChipScale < MinimumChipScale)
            {
                var toTwoMultiplier = MinimumChipScale / currentChipScale;
                _multiplyChipScale(toTwoMultiplier);
            }
        }
    }
}
