using Microsoft.Xna.Framework;
using System;
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

            var leftButton = new SwitchChipsetButton_2(this,0);
            leftButton.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset);
            leftButton.OnMouseReleased += (i) => _buttonClicked(leftButton);
            AddChild(leftButton);

            var rightButton = new SwitchChipsetButton_2(this,1);
            rightButton.SetLocationConfig(70, 0, CoordinateMode.ParentPixelOffset);
            rightButton.OnMouseReleased += (i) => _buttonClicked(rightButton);
            AddChild(rightButton);

            _buttons = new List<SwitchChipsetButton_2>() { leftButton, rightButton };

            var switchArrowButtonLeft = new SpriteMenuItem(this, BuiltInMenuSprites.SwitchBlockSideArrow);
            switchArrowButtonLeft.SetLocationConfig(140, 0, CoordinateMode.ParentPixelOffset);
            //switchArrowButtonLeft.OnMouseReleased += (i) => { UpdateButtonOffset(-1); };
            AddChild(switchArrowButtonLeft);

            var switchArrowButtonRight = new SpriteMenuItem(this, BuiltInMenuSprites.SwitchBlockSideArrow);
            switchArrowButtonRight.FlipHorizontal = true;
            switchArrowButtonRight.SetLocationConfig(140 + switchArrowButtonLeft.GetBaseSize().X, 0, CoordinateMode.ParentPixelOffset);
            //switchArrowButtonRight.OnMouseReleased += (i) => { UpdateButtonOffset(1); };
            AddChild(switchArrowButtonRight);


            var leftText = new ObservableTextMenuItem(ManualDrawLayer.InFrontOf(this, 3));
            leftText.SetLocationConfig(35, 10, CoordinateMode.ParentPixelOffset, true);
            leftText.TextProvider = () => GetBlocksetName(0);
            AddChild(leftText);

            var rightText = new ObservableTextMenuItem(ManualDrawLayer.InFrontOf(this, 3));
            rightText.SetLocationConfig(105, 10, CoordinateMode.ParentPixelOffset, true);
            rightText.TextProvider = () => GetBlocksetName(1);
            AddChild(rightText);

            SwitchSectionBottom = new SpriteMenuItem(this, BuiltInMenuSprites.BlockGreyed) { Visible = false };
            AddChild(SwitchSectionBottom);
        }

        public void ActivateSection(SwitchChipsetButton_2 button)
        {
            button.Activate();
            var section = _subBlocksets[button.Index];

            section.Visible = true;
            section.Enabled = true;
            ActiveSection = section;

            SwitchSectionBottom.Visible = true;
            SwitchSectionBottom.Enabled = true;
        }

        public void DeactivateCurrentSection()
        {
            _buttons.ForEach(b => b.Deactivate());

            if (ActiveSection != null)
            {
                ActiveSection.Visible = false;
                ActiveSection.Enabled = false;
                ActiveSection = null;

                SwitchSectionBottom.Visible = false;
                SwitchSectionBottom.Enabled = false;
            }
        }

        public void CreateSection(string name)
        {
            var blockset = GenerateInternalBlockset();
            AddSection(blockset);
            Model.AddSection(name, blockset.Model);
        }

        public void AddSection(Blockset_2 blockset)
        {
            blockset.Internal = true;
            blockset.Visible = false;
            blockset.Enabled = false;
            blockset.Draggable = false;

            _subBlocksets.Add(blockset);
            AddChild(blockset);
        }

        private void _buttonClicked(SwitchChipsetButton_2 button)
        {
            if (button.Index > _subBlocksets.Count() - 1)
            {
                return;
            }

            var buttonWasOff = !button.ButtonCurrentlyActive;
            DeactivateCurrentSection();

            if (buttonWasOff)
            {
                ActivateSection(button);
            }
        }

        public override void Update(UserInput input)
        {
            _setSubBlocksetPosition();
            base.Update(input);
        }

        private void _setSubBlocksetPosition()
        {
            if(ActiveSection!=null)
            {
                var sizeY = GetSizeWithoutSubBlockset().Y-1;
                ActiveSection.SetLocationConfig(0, sizeY, CoordinateMode.ParentPixelOffset);

                var blocksetY = ActiveSection.GetSizeIncludingBlocks().Y-1;
                SwitchSectionBottom.SetLocationConfig(0, sizeY + blocksetY, CoordinateMode.ParentPixelOffset);
            }
        }

        public IntPoint GetSizeWithoutSubBlockset() => base.GetBaseSize();
        public override IntPoint GetBaseSize() => this.GetSizeWithSubBlockset();

        public void GenerateDefaultSections() => _defaultSwitchSections.ForEach(s => CreateSection(s));
        public string GetBlocksetName(int defaultOffset) => (defaultOffset > Model.SubBlocksets.Count() - 1) ? "" : Model.SubBlocksets[defaultOffset].Item1;
    }

    public class SwitchChipsetButton_2 : SpriteMenuItem
    {
        public int Index;
        public bool ButtonCurrentlyActive;
        public SwitchChipsetButton_2(IHasDrawLayer parent,int index) : base(parent, BuiltInMenuSprites.IfBlockSwitchButton) 
        {
            Index = index;
        }

        public void Activate()
        {
            ButtonCurrentlyActive = true;
            ColorMask = Color.LightGray;
        }

        public void Deactivate()
        {
            ButtonCurrentlyActive = false;
            ColorMask = Color.White;
        }
    }


    public enum ButtonDirection
    {
        Left,
        Right
    }
}
