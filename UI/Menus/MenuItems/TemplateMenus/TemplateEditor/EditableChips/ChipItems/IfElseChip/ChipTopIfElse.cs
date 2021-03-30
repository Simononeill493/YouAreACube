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

        public ChipTopIfElse(IHasDrawLayer parent, ChipData data) : base(parent, data)
        {
            _ifElseSwitch = new ChipIfElseSwitchSection(this, ColorMask, _sectionSwitchedCallback);
        }

        public override void GenerateSubChipsets(Func<EditableChipset> generator)
        {

        }

        public override void DropChipsOn(List<ChipTop> chips, UserInput input)
        {

        }

        private void _sectionSwitchedCallback()
        {

        }
    }
}