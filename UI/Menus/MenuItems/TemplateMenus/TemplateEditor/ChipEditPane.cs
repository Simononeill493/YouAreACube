using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipEditPane : SpriteMenuItem, IEditableChipsetContainer, IChipsetGenerator
    {
        private List<EditableChipset> _allChipsets;
        private List<EditableChipset> _topLevelChipsets;

        public Func<UserInput, bool> IsMouseOverSearchPane;

        public ChipEditPane(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "ChipEditPane")
        {
            DrawLayer = DrawLayers.MenuBehindLayer;

            _allChipsets = new List<EditableChipset>();
            _topLevelChipsets = new List<EditableChipset>();

            var size = GetBaseSize();

            var plusButton = new SpriteMenuItem(this, "PlusButton");
            plusButton.SetLocationConfig(size.X - 9, 0, CoordinateMode.ParentPixelOffset, false);
            plusButton.UpdateDrawLayerCascade(DrawLayer - (DrawLayers.MinLayerDistance * 10));
            plusButton.OnMouseReleased += (i) => _multiplyChipScale(2);
            AddChild(plusButton);

            var minusButton = new SpriteMenuItem(this, "MinusButton_Partial");
            minusButton.SetLocationConfig(size.X - 17, 0, CoordinateMode.ParentPixelOffset, false);
            minusButton.UpdateDrawLayerCascade(DrawLayer - (DrawLayers.MinLayerDistance * 10));
            minusButton.OnMouseReleased += (i) => _multiplyChipScale(0.5f);
            AddChild(minusButton);
        }

        #region chipsetDropped
        private void _chipsetDropped(EditableChipset droppedChipset, UserInput input)
        {
            if (IsMouseOverSearchPane(input))
            {
                _destroyChipset(droppedChipset);
            }
            else
            {
                _attachChipset(droppedChipset, input);
            }

        }
        private void _attachChipset(EditableChipset toAttach, UserInput input)
        {
            var hoveredChipset = _getCurrentlyHoveredChipset(toAttach);
            if (hoveredChipset != null)
            {
                var chipsToDrop = toAttach.PopChips(0);
                hoveredChipset.DropChipsOn(chipsToDrop, input);
                _destroyChipset(toAttach);
            }
            else
            {
                _dropChipsetOnPane(toAttach, input);
            }
        }

        private void _dropChipsetOnPane(EditableChipset toAttach, UserInput input)
        {
            _alignChipsetToPixels(toAttach, GetCurrentSize());
            _setChipsetVisiblity(toAttach);
        }
        private void _alignChipsetToPixels(EditableChipset chip, Point planeSize)
        {
            var pixelDisplacement = this.GetLocationOffset(chip.ActualLocation) / chip.Scale;

            chip.SetLocationConfig(pixelDisplacement, CoordinateMode.ParentPixelOffset, false);
            chip.UpdateDimensionsCascade(ActualLocation, planeSize);
        }

        private EditableChipset _getCurrentlyHoveredChipset(EditableChipset toAttach)
        {
            foreach (var chipset in _topLevelChipsets)
            {
                if (chipset.IsMouseOverAnyChip() & (chipset != toAttach))
                {
                    return chipset;
                }
            }

            return null;
        }
        #endregion

        #region chipsetCreationDestruction
        public void CreateNewChipsetFromSearchChipClick(ChipTop newChip, UserInput input)
        {
            newChip.MultiplyScaleCascade(_chipScaleMultiplier);
            newChip.GenerateSubChipsets(this);

            var newChipset = _createNewChipsetForMouse(input);
            newChipset.AppendChip(newChip);
        }
        public void CreateNewChipsetFromExistingChips(List<ChipTop> newChips, UserInput input, EditableChipset removedFrom)
        {
            var newChipset = _createNewChipsetForMouse(input);
            newChipset.AppendChips(newChips, 0);

            _checkForChipsetGarbageCollection(removedFrom);
        }

        private EditableChipset _createNewChipsetForMouse(UserInput input)
        {
            var newChipset = CreateChipset();
            newChipset.SetLocationConfig(input.MousePos, CoordinateMode.Absolute, centered: true);
            newChipset.UpdateDimensionsCascade(ActualLocation, GetBaseSize());
            newChipset.TryStartDragAtMousePosition(input);
            newChipset.SetContainer(this);
            newChipset.TopLevelRefreshAll = newChipset.RefreshAll;

            return newChipset;
        }
        public EditableChipset CreateChipset()
        {
            var newChipset = new EditableChipset(this, _chipScaleMultiplier, CreateNewChipsetFromExistingChips);
            newChipset.OnEndDrag += (i) => _chipsetDropped(newChipset, i);

            _allChipsets.Add(newChipset);
            return newChipset;
        }

        public void AddChipset(EditableChipset newChipset)
        {
            if(_topLevelChipsets.Contains(newChipset))
            {
                throw new Exception();
            }

            AddChildAfterUpdate(newChipset);
            _topLevelChipsets.Add(newChipset);
        }
        public void RemoveChipset(EditableChipset newChipset)
        {
            if (!_topLevelChipsets.Contains(newChipset))
            {
                throw new Exception();
            }

            RemoveChildAfterUpdate(newChipset);
            _topLevelChipsets.Remove(newChipset);
        }
        private void _destroyChipset(EditableChipset chipset)
        {
            _allChipsets.Remove(chipset);
            chipset.ClearContainer();
            chipset.Dispose();
            
            foreach(var subChipset in chipset.GetSubChipsets())
            {
                _destroyChipset(subChipset);
            }
        }

        private void _checkForChipsetGarbageCollection(EditableChipset toCheck)
        {
            if (toCheck.Chips.Count == 0 & _topLevelChipsets.Contains(toCheck))
            {
                _destroyChipset(toCheck);
            }
        }
        #endregion

        #region visuals
        private const float MinimumChipScale = 2.0f;
        private float _chipScaleMultiplier = 1.0f;

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
            _topLevelChipsets.ForEach(chip => chip.MultiplyScaleCascade(multiplier));
            _updateChildDimensions();
        }

        private void _setChipsetVisibilities() => _topLevelChipsets.ForEach(chip => _setChipsetVisiblity(chip));
        private void _setChipsetVisiblity(EditableChipset chipset)
        {
            if(this.IsIntersectedWith(chipset))
            {
                chipset.Visible = true;
                return;
            }

            foreach(var chip in chipset.Chips)
            {
                if(this.IsIntersectedWith(chip))
                {
                    chipset.Visible = true;
                    return;
                }
            }

            chipset.Visible = false;
        }
        #endregion
    }
}
