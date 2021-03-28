using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{ 
    class ChipTopIfElse : ChipTop
    {
        private List<MenuItem> _yesItems;
        private List<MenuItem> _noItems;

        private ChipIfElseSwitchSection _ifElseSwitch;

        public ChipTopIfElse(IHasDrawLayer parent, ChipData data) : base(parent, data)
        {
            _createInputSections(Chip);
            _createIfElseSwitch();

            _yesItems = new List<MenuItem>() { new SpriteMenuItem(this, "ChipFullGreyed") };
            _noItems = new List<MenuItem>() { new SpriteMenuItem(this, "ChipFull") };
        }

        private void _createIfElseSwitch()
        {
            _ifElseSwitch = new ChipIfElseSwitchSection(this, ColorMask,_sectionSwitchedCallback);
            _addSection(_ifElseSwitch);
        }

        private void _sectionSwitchedCallback(IfElseChipExtensionMode newMode)
        {
            switch (newMode)
            {
                case IfElseChipExtensionMode.None:
                    if (_ifElseSwitch.CurrentMode == IfElseChipExtensionMode.Yes) { _removeSwitchItems(_yesItems); }
                    if (_ifElseSwitch.CurrentMode == IfElseChipExtensionMode.No) { _removeSwitchItems(_noItems); }
                    break;
                case IfElseChipExtensionMode.Yes:
                    if(_ifElseSwitch.CurrentMode== IfElseChipExtensionMode.No) { _removeSwitchItems(_noItems);}
                    _addSwitchItems(_yesItems);
                    break;
                case IfElseChipExtensionMode.No:
                    if (_ifElseSwitch.CurrentMode == IfElseChipExtensionMode.Yes) { _removeSwitchItems(_yesItems); }
                    _addSwitchItems(_noItems);
                    break;
            }

            ChipsetRefreshAllCallback();
        }

        private void _removeSwitchItems(List<MenuItem> items) => items.ForEach(i => _removeSectionAfterUpdate(i));
        private void _addSwitchItems(List<MenuItem> items) => items.ForEach(i => _addSectionAfterUpdate(i));
    }
}