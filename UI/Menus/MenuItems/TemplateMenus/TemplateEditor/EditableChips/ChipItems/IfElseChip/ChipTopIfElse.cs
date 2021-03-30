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

        public ChipTopIfElse(IHasDrawLayer parent, ChipData data) : base(parent, data)
        {
            //_yesChipset = yesChipset;
            //_noChipset = noChipset;

            _yesItems = new List<MenuItem>() { _yesChipset,new SpriteMenuItem(this, "ChipFullGreyed") };
            _noItems = new List<MenuItem>() { _noChipset,new SpriteMenuItem(this, "ChipFullGreyed") };
        }

        private void _sectionSwitchedCallback(IfElseChipExtensionMode newMode)
        {
            switch (newMode)
            {
                case IfElseChipExtensionMode.None:
                    //if (_ifElseSwitch.CurrentMode == IfElseChipExtensionMode.Yes) { _removeSwitchItems(_yesItems); }
                    //if (_ifElseSwitch.CurrentMode == IfElseChipExtensionMode.No) { _removeSwitchItems(_noItems); }
                    _currentSectionBackground = null;
                    _currentChipset = null;
                    break;
                case IfElseChipExtensionMode.Yes:
                    //if(_ifElseSwitch.CurrentMode== IfElseChipExtensionMode.No) { _removeSwitchItems(_noItems);}
                    //_addSwitchItems(_yesItems);
                    _currentSectionBackground = _yesItems.Last();
                    _currentChipset = _yesChipset;
                    break;
                case IfElseChipExtensionMode.No:
                    //if (_ifElseSwitch.CurrentMode == IfElseChipExtensionMode.Yes) { _removeSwitchItems(_yesItems); }
                    //_addSwitchItems(_noItems);
                    _currentSectionBackground = _noItems.Last();
                    _currentChipset = _noChipset;
                    break;
            }

            //ChipsetRefreshAllCallback();
        }

    }
}