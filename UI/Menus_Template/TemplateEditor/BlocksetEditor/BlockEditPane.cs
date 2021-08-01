﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class BlockEditPane : SpriteMenuItem, IBlocksetContainer, IBlocksetGenerator
    {
        public Func<UserInput, bool> IsMouseOverSearchPane;
        public List<Blockset> TopLevelChipsets;

        private const float MinimumChipScale = 2.0f;
        private float _blockScaleMultiplier = 1.0f;
        private List<Blockset> _allBlocksets;


        public BlockEditPane(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "ChipEditPane")
        {
            DrawLayer = DrawLayers.MenuBehindLayer;

            _allBlocksets = new List<Blockset>();
            TopLevelChipsets = new List<Blockset>();

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

        public void LoadTemplateForEditing(CubeTemplate template)
        {
            if(template.Active)
            {
                var generatedChipset = TemplateEditUtils.PrepareChipsetForEditPane(template, this);

                _setChipsetToTopLevel(generatedChipset);
                _dropChipsetOnPane(generatedChipset);
            }
        }

        public void ConfigureNewBlocksetFromSearchPaneClick(BlockTop newBlock, UserInput input)
        {
            newBlock.MultiplyScaleCascade(_blockScaleMultiplier);
            newBlock.BlocksetGenerator = this;
            newBlock.GenerateSubChipsets();

            var newChipset = _createNewChipsetForMouse(input);
            newChipset.AppendBlockToTop(newBlock);
        }
        public void ConfigureNewBlocksetFromExistingBlocks(List<BlockTop> newBlocks, UserInput input, Blockset removedFrom)
        {
            var newBlockset = _createNewChipsetForMouse(input);
            newBlockset.AppendBlocksToTop(newBlocks);

            _checkForChipsetGarbageCollection(removedFrom);
        }
        public Blockset CreateBlockset(string name)
        {
            var newBlockset = new Blockset(name, this, _blockScaleMultiplier, ConfigureNewBlocksetFromExistingBlocks);
            newBlockset.OnEndDrag += (i) => _chipsetDropped(newBlockset, i);

            _allBlocksets.Add(newBlockset);
            return newBlockset;
        }

        public void AddBlockset(Blockset newChipset)
        {
            if (TopLevelChipsets.Contains(newChipset))
            {
                throw new Exception("Tried to add a chipset to the pane that was already there");
            }

            AddChildAfterUpdate(newChipset);
            TopLevelChipsets.Add(newChipset);
        }
        public void RemoveBlockset(Blockset newChipset)
        {
            if (!TopLevelChipsets.Contains(newChipset))
            {
                throw new Exception("Tried to take a chipset from the pane that wasn't there");
            }

            RemoveChildAfterUpdate(newChipset);
            TopLevelChipsets.Remove(newChipset);
        }

        private void _chipsetDropped(Blockset droppedChipset, UserInput input)
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
        private void _attachChipset(Blockset toAttach, UserInput input)
        {
            var hoveredChipset = _getCurrentlyHoveredChipset(toAttach);
            if (hoveredChipset != null)
            {
                var chipsToDrop = toAttach.PopBlocks(0);
                hoveredChipset.DropBlocksOnThis(chipsToDrop, input);
                _destroyChipset(toAttach);
            }
            else
            {
                _dropChipsetOnPane(toAttach);
            }
        }

        private void _dropChipsetOnPane(Blockset toAttach)
        {
            _alignChipsetToPixels(toAttach, GetCurrentSize());
            _setChipsetVisiblity(toAttach);
        }
        private void _alignChipsetToPixels(Blockset chip, IntPoint planeSize)
        {
            var pixelDisplacement = this.GetLocationOffset(chip.ActualLocation) / chip.Scale;

            chip.SetLocationConfig(pixelDisplacement, CoordinateMode.ParentPixelOffset, false);
            chip.UpdateDimensionsCascade(ActualLocation, planeSize);
        }

        private Blockset _getCurrentlyHoveredChipset(Blockset toAttach)
        {
            foreach (var chipset in TopLevelChipsets)
            {
                if (chipset.IsMouseOverAnyBlock() & (chipset != toAttach))
                {
                    return chipset;
                }
            }

            return null;
        }
        private Blockset _createNewChipsetForMouse(UserInput input)
        {
            var newChipset = CreateBlockset("Chipset_" + _allBlocksets.Count+1);
            _setChipsetToTopLevel(newChipset);

            newChipset.SetLocationConfig(input.MousePos, CoordinateMode.Absolute, centered: true);
            newChipset.UpdateDimensionsCascade(ActualLocation, GetBaseSize());
            newChipset.TryStartDragAtMousePosition(input);

            return newChipset;
        }

        protected void _setChipsetToTopLevel(Blockset chipset)
        {
            chipset.SetContainer(this);
            chipset.TopLevelRefreshAll = chipset.RefreshAll;
        }
        private void _destroyChipset(Blockset chipset)
        {
            _allBlocksets.Remove(chipset);
            chipset.ClearContainer();
            chipset.Dispose();
            
            foreach(var subChipset in chipset.GetSubBlocksets())
            {
                _destroyChipset(subChipset);
            }
        }
        private void _checkForChipsetGarbageCollection(Blockset toCheck)
        {
            if (toCheck.Blocks.Count == 0 & TopLevelChipsets.Contains(toCheck))
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
            var currentChipScale = GenerateScaleFromMultiplier(_blockScaleMultiplier);
            if (currentChipScale < MinimumChipScale)
            {
                var toTwoMultiplier = MinimumChipScale / currentChipScale;
                _multiplyChipScale(toTwoMultiplier);
            }
        }
        private void _multiplyChipScale(float multiplier)
        {
            _blockScaleMultiplier *= multiplier;
            TopLevelChipsets.ForEach(chip => chip.MultiplyScaleCascade(multiplier));
            _updateChildDimensions();
        }
        private void _setChipsetVisibilities() => TopLevelChipsets.ForEach(chip => _setChipsetVisiblity(chip));
        private void _setChipsetVisiblity(Blockset chipset)
        {
            if(this.IsIntersectedWith(chipset))
            {
                chipset.Visible = true;
                return;
            }

            foreach(var chip in chipset.Blocks)
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
