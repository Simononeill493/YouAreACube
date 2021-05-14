using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class ChipEditPane : SpriteMenuItem, IEditableChipsetContainer, IChipsetGenerator
    {
        public Func<UserInput, bool> IsMouseOverSearchPane;
        public List<EditableChipset> TopLevelChipsets;

        private const float MinimumChipScale = 2.0f;
        private float _chipScaleMultiplier = 1.0f;
        private List<EditableChipset> _allChipsets;


        public ChipEditPane(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "ChipEditPane")
        {
            DrawLayer = DrawLayers.MenuBehindLayer;

            _allChipsets = new List<EditableChipset>();
            TopLevelChipsets = new List<EditableChipset>();

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

        public void LoadTemplateForEditing(BlockTemplate template)
        {
            var generatedChipset = TemplateEditUtils.PrepareChipsetForEditPane(template, this);

            _setChipsetToTopLevel(generatedChipset);
            _dropChipsetOnPane(generatedChipset);
        }

        public void ConfigureNewChipsetFromSearchPaneClick(ChipTop newChip, UserInput input)
        {
            newChip.MultiplyScaleCascade(_chipScaleMultiplier);
            newChip.SetGenerator(this);
            newChip.GenerateSubChipsets();

            var newChipset = _createNewChipsetForMouse(input);
            newChipset.AppendToTop(newChip);
        }
        public void ConfigureNewChipsetFromExistingChips(List<ChipTop> newChips, UserInput input, EditableChipset removedFrom)
        {
            var newChipset = _createNewChipsetForMouse(input);
            newChipset.AppendChips(newChips, 0);

            _checkForChipsetGarbageCollection(removedFrom);
        }
        public EditableChipset CreateChipset(string name)
        {
            var newChipset = new EditableChipset(name, this, _chipScaleMultiplier, ConfigureNewChipsetFromExistingChips);
            newChipset.OnEndDrag += (i) => _chipsetDropped(newChipset, i);

            _allChipsets.Add(newChipset);
            return newChipset;
        }

        public void AddChipset(EditableChipset newChipset)
        {
            if (TopLevelChipsets.Contains(newChipset))
            {
                throw new Exception("Tried to add a chipset to the pane that was already there");
            }

            AddChildAfterUpdate(newChipset);
            TopLevelChipsets.Add(newChipset);
        }
        public void RemoveChipset(EditableChipset newChipset)
        {
            if (!TopLevelChipsets.Contains(newChipset))
            {
                throw new Exception("Tried to take a chipset from the pane that wasn't there");
            }

            RemoveChildAfterUpdate(newChipset);
            TopLevelChipsets.Remove(newChipset);
        }

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
                _dropChipsetOnPane(toAttach);
            }
        }

        private void _dropChipsetOnPane(EditableChipset toAttach)
        {
            _alignChipsetToPixels(toAttach, GetCurrentSize());
            _setChipsetVisiblity(toAttach);
        }
        private void _alignChipsetToPixels(EditableChipset chip, IntPoint planeSize)
        {
            var pixelDisplacement = this.GetLocationOffset(chip.ActualLocation) / chip.Scale;

            chip.SetLocationConfig(pixelDisplacement, CoordinateMode.ParentPixelOffset, false);
            chip.UpdateDimensionsCascade(ActualLocation, planeSize);
        }

        private EditableChipset _getCurrentlyHoveredChipset(EditableChipset toAttach)
        {
            foreach (var chipset in TopLevelChipsets)
            {
                if (chipset.IsMouseOverAnyChip() & (chipset != toAttach))
                {
                    return chipset;
                }
            }

            return null;
        }
        private EditableChipset _createNewChipsetForMouse(UserInput input)
        {
            var newChipset = CreateChipset("Chipset_" + _allChipsets.Count+1);
            _setChipsetToTopLevel(newChipset);

            newChipset.SetLocationConfig(input.MousePos, CoordinateMode.Absolute, centered: true);
            newChipset.UpdateDimensionsCascade(ActualLocation, GetBaseSize());
            newChipset.TryStartDragAtMousePosition(input);

            return newChipset;
        }

        protected void _setChipsetToTopLevel(EditableChipset chipset)
        {
            chipset.SetContainer(this);
            chipset.TopLevelRefreshAll = chipset.RefreshAll;
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
            if (toCheck.Chips.Count == 0 & TopLevelChipsets.Contains(toCheck))
            {
                _destroyChipset(toCheck);
            }
        }

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
            TopLevelChipsets.ForEach(chip => chip.MultiplyScaleCascade(multiplier));
            _updateChildDimensions();
        }
        private void _setChipsetVisibilities() => TopLevelChipsets.ForEach(chip => _setChipsetVisiblity(chip));
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
    }
}
