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
            if (MenuScreen.IsUserDragging) { return; }
            _createAndAttachNewChipsetToMouse(preview,input);
        }

        private void _chipsetDropped(EditableChipset releasedChipset, UserInput input)
        {
            if (IsMouseOverSearchPane(input))
            {
                _deleteChipset(releasedChipset);
            }
            else
            {
                _attachChipset(releasedChipset, input);
            }
        }
        private void _attachChipset(EditableChipset releasedChipset, UserInput input)
        {
            var (chipsetDroppedOn, index, isBottom) = _getFirstChipsetThatMouseIsOver(input, releasedChipset);
            if (chipsetDroppedOn != null)
            {
                _attachChipsetToOtherChipset(releasedChipset, chipsetDroppedOn, index + (isBottom ? 1 : 0));
            }
            else
            {
                _attachChipsetToPane(releasedChipset);
            }
        }

        private void _attachChipsetToPane(EditableChipset releasedChipset)
        {
            releasedChipset.LiftChipsCallback = _chipsLiftedFromPane;

            _alignChipsetToPixels(releasedChipset, GetCurrentSize());
            _setChipsetVisiblity(releasedChipset);
        }
        private void _attachChipsetToOtherChipset(EditableChipset releasedChipset, EditableChipset chipsetDroppedOn, int index)
        {
            var chipsToAppend = releasedChipset.PopChips(0);
            chipsetDroppedOn.AppendChips(chipsToAppend, index);
            _deleteChipset(releasedChipset);
        }

        private (EditableChipset chipset, int chipIndex, bool isBottom) _getFirstChipsetThatMouseIsOver(UserInput input, EditableChipset chipsetJustReleased)
        {
            foreach (var chipset in _chipsets)
            {
                if (chipset != chipsetJustReleased)
                {
                    var pos = chipset.GetChipIndexThatMouseIsOver(input);
                    if (pos.index != -1)
                    {
                        var (subChipset, subIndex, bottom) = chipset.GetSubChipThatMouseIsOverIfAny(input,pos.index);
                        if(subChipset !=null)
                        {
                            return (subChipset, subIndex, bottom);
                        }
                        else
                        {
                            return (chipset, pos.index, pos.bottom);
                        }
                    }
                }
            }

            return (null, -1, false);
        }

        private void _alignChipsetToPixels(EditableChipset chip, Point planeSize)
        {
            var pixelDisplacement = this.GetLocationOffset(chip.ActualLocation) / chip.Scale;

            chip.SetLocationConfig(pixelDisplacement, CoordinateMode.ParentPixelOffset, false);
            chip.UpdateDimensionsCascade(ActualLocation, planeSize);
        }
        private void _attachNewChipsetToMouse(EditableChipset chipset, UserInput input)
        {
            chipset.TryStartDrag(input, chipset.DefaultMouseDragOffset);
            chipset.OnEndDrag += (i) => _chipsetDropped(chipset, i);

            _chipsets.Add(chipset);
            AddChildAfterUpdate(chipset);
        }

        private EditableChipset _createChipset() => _createChipset(new List<ChipTop>());
        private EditableChipset _createChipset(ChipTop chip) => _createChipset(new List<ChipTop>() { chip });
        private EditableChipset _createChipset(List<ChipTop> chips) => EditableChipsetFactory.Create(this, chips, _chipScaleMultiplier);
        private void _deleteChipset(EditableChipset chipset)
        {
            _chipsets.Remove(chipset);
            RemoveChildAfterUpdate(chipset);

            chipset.Dispose();
        }

        private void _chipsLiftedFromPane(List<ChipTop> chips, UserInput input) => _attachNewChipsetToMouse(_createChipset(chips), input);
        private void _createAndAttachNewChipsetToMouse(ChipPreview preview, UserInput input) => _attachNewChipsetToMouse(_createChipset(preview.GenerateChip(_chipScaleMultiplier, _createChipset)), input);

        #region visuals
        protected override void _updateChildDimensions()
        {
            _pushChipScalingUpIfTooSmall();
            base._updateChildDimensions();
            _setChipsetVisibilities();
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
        private void _multiplyChipScale(float multiplier)
        {
            _chipScaleMultiplier *= multiplier;
            _chipsets.ForEach(chip => chip.MultiplyScaleCascade(multiplier));
            _updateChildDimensions();
        }

        private void _setChipsetVisibilities() => _chipsets.ForEach(chip => _setChipsetVisiblity(chip));
        private void _setChipsetVisiblity(EditableChipset chip) => chip.Visible = this.IsIntersectedWith(chip.GetFullRect());
        #endregion
    }
}
