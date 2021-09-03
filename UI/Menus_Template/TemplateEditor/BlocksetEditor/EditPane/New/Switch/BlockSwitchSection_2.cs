﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockSwitchSection_2 : SpriteMenuItem
    {
        public BlockModel Model;
        private List<string> _defaultSwitchSections;

        public bool IsAnySectionActivated => ActiveSection != null;
        public Blockset_2 ActiveSection;
        public SpriteMenuItem SwitchSectionBottom;

        private List<Blockset_2> _subBlocksets;

        private List<SwitchChipsetButton_2> _buttons;

        public Func<Blockset_2> GenerateInternalBlockset;
        public void SetGenerator(Func<Blockset_2> generator) => GenerateInternalBlockset = generator;

        public BlockSwitchSection_2(IHasDrawLayer parent, BlockModel model, List<string> defaultSwitchSections) : base(parent, BuiltInMenuSprites.BlockBottom)
        {
            Model = model;
            _defaultSwitchSections = defaultSwitchSections;
            _subBlocksets = new List<Blockset_2>();
            _buttons = new List<SwitchChipsetButton_2>();

            var leftButton = _addItem(new SwitchChipsetButton_2(this,0), 0, 0, CoordinateMode.ParentPixelOffset);
            var rightButton = _addItem(new SwitchChipsetButton_2(this, 1), 70, 0, CoordinateMode.ParentPixelOffset);
            var switchArrowButtonLeft = _addSpriteItem(BuiltInMenuSprites.SwitchBlockSideArrow, 140, 0, CoordinateMode.ParentPixelOffset, false);
            var switchArrowButtonRight = _addSpriteItem(BuiltInMenuSprites.SwitchBlockSideArrow, 140 + switchArrowButtonLeft.GetBaseSize().X, 0, CoordinateMode.ParentPixelOffset, false);
            var leftText = _addItem(new ObservableTextMenuItem(ManualDrawLayer.InFrontOf(this, 3)), 35, 10, CoordinateMode.ParentPixelOffset, true);
            var rightText = _addItem(new ObservableTextMenuItem(ManualDrawLayer.InFrontOf(this, 3)), 105, 10, CoordinateMode.ParentPixelOffset, true);
            SwitchSectionBottom = _addSpriteItem(BuiltInMenuSprites.BlockGreyed, 0, 0, CoordinateMode.Absolute, false);

            leftButton.OnMouseReleased += (i) => _buttonClicked(leftButton);
            rightButton.OnMouseReleased += (i) => _buttonClicked(rightButton);
            leftText.TextProvider = () => GetBlocksetName(0);
            rightText.TextProvider = () => GetBlocksetName(1);
            switchArrowButtonRight.FlipHorizontal = true;
            SwitchSectionBottom.Visible = false;

            _buttons.Add(leftButton);
            _buttons.Add(rightButton);
        }

        private void _buttonClicked(SwitchChipsetButton_2 button)
        {
            if (button.Index > _subBlocksets.Count() - 1)
            {
                return;
            }

            var buttonWasOff = !button.ButtonCurrentlyActive;

            if (IsAnySectionActivated)
            {
                DeactivateCurrentSection();
            }

            if (buttonWasOff)
            {
                ActivateSection(button);
            }
        }


        public void ActivateSection(SwitchChipsetButton_2 button)
        {
            var section = _subBlocksets[button.Index];

            button.Activate();
            section.ShowAndEnable();
            SwitchSectionBottom.ShowAndEnable();

            ActiveSection = section;
        }
        
        public void DeactivateCurrentSection()
        {
            _buttons.ForEach(b => b.Deactivate());
            ActiveSection.HideAndDisable();
            SwitchSectionBottom.HideAndDisable();

            ActiveSection = null;
        }

        public void CreateAndAddSection(string name)
        {
            var blockset = GenerateInternalBlockset();
            AddSection(blockset);
            Model.AddSection(name, blockset.Model);
        }

        public void AddSection(Blockset_2 blockset)
        {
            blockset.HideAndDisable();
            blockset.Draggable = false;
            blockset.Internal = true;

            _subBlocksets.Add(blockset);
            AddChild(blockset);
        }

        public override void Update(UserInput input)
        {
            if(IsAnySectionActivated)
            {
                _setSubBlocksetPosition();
            }
            base.Update(input);
        }

        private void _setSubBlocksetPosition()
        {
            var sizeY = GetSizeWithoutSubBlockset().Y-1;
            ActiveSection.SetLocationConfig(0, sizeY, CoordinateMode.ParentPixelOffset);

            var blocksetY = ActiveSection.GetSizeIncludingBlocks().Y-1;
            SwitchSectionBottom.SetLocationConfig(0, sizeY + blocksetY, CoordinateMode.ParentPixelOffset);
        }

        public IntPoint GetSizeWithoutSubBlockset() => base.GetBaseSize();
        public override IntPoint GetBaseSize() => this.GetSizeWithSubBlockset();

        public void GenerateDefaultSections() => _defaultSwitchSections.ForEach(s => CreateAndAddSection(s));
        public string GetBlocksetName(int defaultOffset) => (defaultOffset > Model.SubBlocksets.Count() - 1) ? "" : Model.SubBlocksets[defaultOffset].Item1;
    }
}
