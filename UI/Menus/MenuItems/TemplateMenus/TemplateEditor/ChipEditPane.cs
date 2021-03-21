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
        private float _chipScaleMultiplier = 1.0f;

        private List<EditableChipset> _chipsets;

        public Func<UserInput, bool> IsMouseOverSearchPane;
        
        public ChipEditPane(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "ChipEditPane")
        {
            DrawLayer = DrawLayers.MenuBehindLayer;

            _chipsets = new List<EditableChipset>();

            var size = GetBaseSize();

            var plusButton = new SpriteMenuItem(this, "PlusButton");
            plusButton.SetLocationConfig(size.X - 9, 0, CoordinateMode.ParentPixelOffset, false);
            plusButton.UpdateDrawLayerCascade(DrawLayer - (DrawLayers.MinLayerDistance*10));
            plusButton.OnMouseReleased += (i) => _multiplyChipScale(2);
            AddChild(plusButton);

            var minusButton = new SpriteMenuItem(this, "MinusButton_Partial");
            minusButton.SetLocationConfig(size.X - 17, 0, CoordinateMode.ParentPixelOffset, false);
            minusButton.UpdateDrawLayerCascade(DrawLayer - (DrawLayers.MinLayerDistance * 10));
            minusButton.OnMouseReleased += (i) => _multiplyChipScale(0.5f);
            AddChild(minusButton);
        }

        public void TryCreateChipsetFromSearchPane(ChipPreview preview, UserInput input)
        {
            if (MenuScreen.UserDragging) { return; }
            _createAndAttachNewChipset(preview,input);
        }

        private EditableChipset _createChipset(ChipItem chip) => _createChipset(new List<ChipItem>() { chip });
        private EditableChipset _createChipset(List<ChipItem> chips)
        {
            var chipset = new EditableChipset(this, _chipScaleMultiplier);

            chipset.UpdateDimensions(ActualLocation, GetCurrentSize());
            chipset.AppendChips(chips, 0);

            chipset.CreateNewChipsetInEditPane = _createAndAttachSplitChipset;
            chipset.OnEndDrag += (i) => _chipsetReleased(chipset, i);

            return chipset;
        }
        private void _attachChipset(EditableChipset chipset,UserInput input)
        {
            chipset.TryStartDrag(input, chipset.DefaultMouseDragOffset);

            _chipsets.Add(chipset);
            AddChildAfterUpdate(chipset);
        }
        private void _deleteChipset(EditableChipset chipset)
        {
            _chipsets.Remove(chipset);
            RemoveChildAfterUpdate(chipset);
            
            chipset.Dispose();
        }
        
        private void _chipsetReleased(EditableChipset releasedChipset, UserInput input)
        {
            if (IsMouseOverSearchPane(input))
            {
                _deleteChipset(releasedChipset);
            }
            else
            {
                _reattachChipset(releasedChipset, input);
            }
        }
        private void _reattachChipset(EditableChipset releasedChipset, UserInput input)
        {
            var (chipsetDroppedOn, index) = _getFirstChipsetThatMouseIsOver(input, releasedChipset);
            if (chipsetDroppedOn != null)
            {
                chipsetDroppedOn.AppendChips(releasedChipset.Chips, index);
                _deleteChipset(releasedChipset);
            }
            else
            {
                _attachChipsetToPane(releasedChipset, GetCurrentSize());
                _setChipsetVisiblity(releasedChipset);
            }
        }

        private (EditableChipset chipset, int chipIndex) _getFirstChipsetThatMouseIsOver(UserInput input, EditableChipset chipsetJustReleased)
        {
            foreach (var chipset in _chipsets)
            {
                if (chipset != chipsetJustReleased)
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

        protected override void _updateChildDimensions()
        {
            _pushChipScalingUpIfTooSmall();
            base._updateChildDimensions();
            _setChipsetVisibilities();
        }

        private void _attachChipsetToPane(EditableChipset chip, Point planeSize)
        {
            var pixelDisplacement = this.GetLocationOffset(chip.ActualLocation) / chip.Scale;

            chip.SetLocationConfig(pixelDisplacement, CoordinateMode.ParentPixelOffset, false);
            chip.UpdateDimensionsCascade(ActualLocation, planeSize);
        }

        private void _multiplyChipScale(float multiplier)
        {
            _chipScaleMultiplier *= multiplier;
            _chipsets.ForEach(chip => chip.MultiplyScaleCascade(multiplier));
            _updateChildDimensions();
        }
        private void _pushChipScalingUpIfTooSmall()
        {
            var currentChipScale = GenerateScaleFromMultiplier(_chipScaleMultiplier);
            if (currentChipScale < MinimumChipScale)
            {
                var toTwoMultiplier = MinimumChipScale / currentChipScale;
                _multiplyChipScale(toTwoMultiplier);
            }
        }

        private void _createAndAttachSplitChipset(List<ChipItem> chips, UserInput input) => _attachChipset(_createChipset(chips), input);
        private void _createAndAttachNewChipset(ChipPreview preview, UserInput input) => _attachChipset(_createChipset(preview.GenerateChip(_chipScaleMultiplier)), input);

        private void _setChipsetVisibilities() => _chipsets.ForEach(chip => _setChipsetVisiblity(chip));
        private void _setChipsetVisiblity(EditableChipset chip) => chip.Visible = this.IsIntersectedWith(chip.GetFullRect());
    }
}
