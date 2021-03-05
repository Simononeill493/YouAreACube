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

        public MenuItem Trash;
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

        public void TryCreateChipset(ChipPreviewSmall preview, UserInput input)
        {
            if (MenuScreen.UserDragging) { return; }

            var chipset = EditableChipset.CreateAtMouse(input, this);
            chipset.AddInitialChip(preview.Chip,ChipScaleMultiplier);

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
            if (Trash.IsMouseOver(input))
            {
                RemoveChipset(chipset);
            }
            else
            {
                var (chipDropped,index) = _getChipsetHoveringOn(input,chipset);
                if(chipDropped != null)
                {
                    RemoveChipset(chipset);
                    chipDropped.AppendChipset(chipset, index);
                }
                else
                {
                    _attachChipsetToPane(chipset, GetCurrentSize());
                    _setChipsetVisiblity(chipset);
                }
            }
        }

        private (EditableChipset chipset,int placement) _getChipsetHoveringOn(UserInput input,EditableChipset currentlyHovering)
        {
            foreach(var chipset in Chipsets)
            {
                if(chipset!=currentlyHovering)
                {
                    var pos = chipset.GetHoveredChip(input);
                    if(pos != -1)
                    {
                        Console.WriteLine(pos);
                        return (chipset, pos);
                    }
                }
            }

            return (null, -1);
        }

        protected override void _updateChildDimensions()
        {
            _pushChipScalingUpIfTooSmall();
            base._updateChildDimensions();
            _setChipsetVisibilities();
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

        private void _pushChipScalingUpIfTooSmall()
        {
            var newChipScale = GenerateScaleFromMultiplier(ChipScaleMultiplier);
            if (newChipScale < MinimumChipScale)
            {
                var toTwoMultiplier = MinimumChipScale / newChipScale;
                _multiplyChipScale(toTwoMultiplier);
            }
        }
    }
}
