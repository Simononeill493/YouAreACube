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

        private Func<UserInput, bool> _isMouseOverSearchPane;
        public void SetSearchPaneMouseOverCallback(Func<UserInput, bool> callback)=>_isMouseOverSearchPane = callback;
        
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

            var minusButton = new SpriteMenuItem(this, "MinusButton");
            minusButton.SetLocationConfig(size.X - 17, 0, CoordinateMode.ParentPixelOffset, false);
            minusButton.UpdateDrawLayerCascade(DrawLayer - (DrawLayers.MinLayerDistance * 10));
            minusButton.OnMouseReleased += (i) => _multiplyChipScale(0.5f);
            AddChild(minusButton);
        }

        public void TryCreateChipsetFromSearchPane(ChipPreviewSmall preview, UserInput input)
        {
            if (MenuScreen.UserDragging) { return; }
            _createAndAttachNewChipset(preview,input);
        }

        private void _createAndAttachNewChipset(ChipPreviewSmall preview, UserInput input)
        {
            var chipHoverLayer = ManualDrawLayer.Create(DrawLayers.MenuHoverLayer);
            var chip = new ChipPreviewLarge(chipHoverLayer, preview.Chip);
            chip.MultiplyScaleCascade(_chipScaleMultiplier);

            var chipList = new List<ChipPreviewLarge>() { chip };
            var chipset = _createChipset(chipList,input);
            _attachChipset(chipset, input);
        }

        private void _createAndAttachSplitChipset(List<ChipPreviewLarge> chips, UserInput input)
        {
            var chipset = _createChipset(chips, input);
            _attachChipset(chipset,input);
        }

        private void _attachChipset(EditableChipset chipset,UserInput input)
        {
            /*foreach (var chip in chipset.GetAttachedChips())
            {
                chip.UpdateDrawLayerCascade(chipset.DrawLayer - DrawLayers.MinLayerDistance);
            }*/

            chipset.TryStartDrag(input, chipset.GetAttachedChips().First().GetCurrentSize() / 2);

            _chipsets.Add(chipset);
            AddChildAfterUpdate(chipset);
        }

        private EditableChipset _createChipset(List<ChipPreviewLarge> chips,UserInput input)
        {
            var chipset = new EditableChipset(this, _chipScaleMultiplier);

            chipset.UpdateDimensions(ActualLocation, GetCurrentSize());
            chipset.AppendChips(chips, 0);

            chipset.SetCreateNewChipsetCallback(_createAndAttachSplitChipset);
            chipset.OnEndDrag += (i) => _chipsetReleased(chipset, i);
            
            return chipset;
        }

        private void _deleteChipset(EditableChipset chipset)
        {
            _chipsets.Remove(chipset);
            RemoveChildAfterUpdate(chipset);
            
            chipset.Dispose();
        }
        
        private void _chipsetReleased(EditableChipset releasedChipset, UserInput input)
        {
            if (_isMouseOverSearchPane(input))
            {
                _deleteChipset(releasedChipset);
            }
            else
            {
                var (chipsetDroppedOn,index) = _getFirstChipsetThatMouseIsOver(input,releasedChipset);
                if(chipsetDroppedOn != null)
                {
                    chipsetDroppedOn.AppendChips(releasedChipset.GetAttachedChips(), index);
                    _deleteChipset(releasedChipset);
                }
                else
                {
                    _attachChipsetToPane(releasedChipset, GetCurrentSize());
                    _setChipsetVisiblity(releasedChipset);
                }
            }
        }

        protected override void _updateChildDimensions()
        {
            _pushChipScalingUpIfTooSmall();
            base._updateChildDimensions();
            _setChipsetVisibilities();
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

        private void _attachChipsetToPane(EditableChipset chip, Point planeSize)
        {
            var pixelDisplacement = this.GetLocationOffset(chip.ActualLocation) / chip.Scale;

            chip.SetLocationConfig(pixelDisplacement, CoordinateMode.ParentPixelOffset, false);
            chip.UpdateDimensionsCascade(ActualLocation, planeSize);
        }

        private void _multiplyChipScale(float multiplier)
        {
            _chipScaleMultiplier *= multiplier;

            foreach (var chip in _chipsets)
            {
                chip.MultiplyScaleCascade(multiplier);
            }

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

        private void _setChipsetVisibilities()
        {
            foreach (var chip in _chipsets)
            {
                _setChipsetVisiblity(chip);
            }
        }
        private void _setChipsetVisiblity(EditableChipset chip)
        {
            var isIntersected = this.IsIntersectedWith(chip.GetFullRect());
            chip.Visible = isIntersected;
        }
    }
}
