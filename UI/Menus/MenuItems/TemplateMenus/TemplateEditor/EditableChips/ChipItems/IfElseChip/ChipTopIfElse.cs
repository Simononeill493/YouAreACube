using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{ 
    class ChipTopIfElse : ChipTop
    {
        private ChipIfElseSwitchSection _ifElseSwitch;

        private List<MenuItem> _yesItems;
        private List<MenuItem> _noItems;

        private EditableChipset _yesChipset;
        private EditableChipset _noChipset;

        private MenuItem _currentSectionBackground;
        private EditableChipset _currentChipset;

        public ChipTopIfElse(IHasDrawLayer parent, ChipData data,EditableChipset yesChipset,EditableChipset noChipset) : base(parent, data)
        {
            _createInputSections(Chip);
            _createIfElseSwitch();

            _yesChipset = yesChipset;
            _noChipset = noChipset;

            _yesItems = new List<MenuItem>() { _yesChipset,new SpriteMenuItem(this, "ChipFullGreyed") };
            _noItems = new List<MenuItem>() { _noChipset,new SpriteMenuItem(this, "ChipFullGreyed") };
        }

        public override (EditableChipset chipset, int index, bool bottom) GetSubChipThatMouseIsOverIfAny(UserInput input)
        {
            if (_ifElseSwitch.CurrentMode != IfElseChipExtensionMode.None)
            {
                if(_currentSectionBackground.MouseHovering)
                {
                    return (_currentChipset, 0, false);
                }
            }

            return (null, -1, false);
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
                    _currentSectionBackground = null;
                    _currentChipset = null;
                    break;
                case IfElseChipExtensionMode.Yes:
                    if(_ifElseSwitch.CurrentMode== IfElseChipExtensionMode.No) { _removeSwitchItems(_noItems);}
                    _addSwitchItems(_yesItems);
                    _currentSectionBackground = _yesItems.Last();
                    _currentChipset = _yesChipset;
                    break;
                case IfElseChipExtensionMode.No:
                    if (_ifElseSwitch.CurrentMode == IfElseChipExtensionMode.Yes) { _removeSwitchItems(_yesItems); }
                    _addSwitchItems(_noItems);
                    _currentSectionBackground = _noItems.Last();
                    _currentChipset = _noChipset;
                    break;
            }

            ChipsetRefreshAllCallback();
        }

        private void _removeSwitchItems(List<MenuItem> items) => items.ForEach(i => _removeSectionAfterUpdate(i));
        private void _addSwitchItems(List<MenuItem> items) => items.ForEach(i => _addSectionAfterUpdate(i));
    }
}