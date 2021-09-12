using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockSwitchSection : SpriteMenuItem
    {
        public BlockModel Model;
        private List<string> _defaultSwitchSections;

        public bool IsAnySectionActivated => ActiveSection != null;
        public Blockset ActiveSection;
        public SpriteMenuItem SwitchSectionBottom;

        public IEnumerable<Blockset> SubBlocksets => Model.SubBlocksets.Select(b => BlocksetEditPane.Blocksets[b.Item2]);

        private List<SwitchChipsetButton> _buttons;

        public Func<Blockset> GenerateInternalBlockset;
        public void SetGenerator(Func<Blockset> generator) => GenerateInternalBlockset = generator;

        private int _currentOffset;

        public BlockSwitchSection(IHasDrawLayer parent, BlockModel model, List<string> defaultSwitchSections) : base(parent, BuiltInMenuSprites.BlockBottom)
        {
            Model = model;
            _defaultSwitchSections = defaultSwitchSections;
            _buttons = new List<SwitchChipsetButton>();

            var leftButton = _addItem(new SwitchChipsetButton(this,0), 0, 0, CoordinateMode.ParentPixelOffset);
            var rightButton = _addItem(new SwitchChipsetButton(this, 1), 70, 0, CoordinateMode.ParentPixelOffset);
            var switchArrowButtonLeft = _addSpriteItem(BuiltInMenuSprites.SwitchBlockSideArrow, 140, 0, CoordinateMode.ParentPixelOffset, false);
            var switchArrowButtonRight = _addSpriteItem(BuiltInMenuSprites.SwitchBlockSideArrow, 140 + switchArrowButtonLeft.GetBaseSize().X, 0, CoordinateMode.ParentPixelOffset, false);
            var leftText = _addItem(new ObservableTextMenuItem(ManualDrawLayer.InFrontOf(this, 3)), 35, 10, CoordinateMode.ParentPixelOffset, true);
            var rightText = _addItem(new ObservableTextMenuItem(ManualDrawLayer.InFrontOf(this, 3)), 105, 10, CoordinateMode.ParentPixelOffset, true);
            SwitchSectionBottom = _addSpriteItem(BuiltInMenuSprites.BlockGreyed, 0, 0, CoordinateMode.Absolute, false);

            leftButton.OnMouseReleased += (i) => _buttonClicked(leftButton);
            rightButton.OnMouseReleased += (i) => _buttonClicked(rightButton);
            switchArrowButtonLeft.OnMouseReleased += (i) => ChangeOffset(-1);
            switchArrowButtonRight.OnMouseReleased += (i) => ChangeOffset(1);
            leftText.TextProvider = () => GetBlocksetName(0);
            rightText.TextProvider = () => GetBlocksetName(1);

            switchArrowButtonRight.FlipHorizontal = true;
            SwitchSectionBottom.Visible = false;

            _buttons.Add(leftButton);
            _buttons.Add(rightButton);
        }

        public void ChangeOffset(int amount)
        {
            var newOffset = _currentOffset + amount;
            if(newOffset<0 | (newOffset+_buttons.Count())>Model.SubBlocksets.Count())
            {
                return;
            }

            _currentOffset = newOffset;
            if(IsAnySectionActivated)
            {
                _refreshOpenSection();
            }
        }

        private void _refreshOpenSection()
        {
            var currentActiveButton = _buttons.FirstOrDefault(b => b.ButtonCurrentlyActive);

            DeactivateCurrentSection();
            ActivateSection(currentActiveButton);
        }

        public void ActivateSection(SwitchChipsetButton button)
        {
            var section = SubBlocksets.ToList()[button.Index + _currentOffset];

            button.Activate();

            SwitchSectionBottom.ShowAndEnable();
            section.ShowAndEnable();
            ActiveSection = section;
        }

        public void DeactivateCurrentSection()
        {
            _buttons.ForEach(b => b.Deactivate());
            SwitchSectionBottom.HideAndDisable();

            ActiveSection.HideAndDisable();
            ActiveSection = null;
        }

        public void CreateAndAddSection(string name)
        {
            var blockset = GenerateInternalBlockset();
            _initializeSection(blockset);
            Model.AddSection(name, blockset.Model);
        }

        public void InitializeAllSections() => SubBlocksets.ToList().ForEach(b => _initializeSection(b));

        private void _initializeSection(Blockset blockset)
        {
            blockset.IsInternal = true;
            blockset.VisualParent = this;
            blockset.InternalBlocksetBottom = SwitchSectionBottom;
            blockset.HideAndDisable();
        }


        private void _buttonClicked(SwitchChipsetButton button)
        {
            if (button.Index > SubBlocksets.Count() - 1)
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

        public IntPoint GetSizeWithoutSubBlockset() => base.GetBaseSize();
        public override IntPoint GetBaseSize() => this.GetSizeWithSubBlockset();

        public void GenerateDefaultSections() => _defaultSwitchSections.ForEach(s => CreateAndAddSection(s));
        public string GetBlocksetName(int defaultOffset) => (defaultOffset+_currentOffset > Model.SubBlocksets.Count() - 1) ? "" : Model.SubBlocksets[defaultOffset + _currentOffset].Item1;
    }
}
