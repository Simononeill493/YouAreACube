using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{ 
    class ChipTopIfElse : ChipTop
    {
        public ChipTopIfElse(IHasDrawLayer parent, ChipData data) : base(parent, data)
        {
            _createInputSections(Chip);
        }
    }
}
