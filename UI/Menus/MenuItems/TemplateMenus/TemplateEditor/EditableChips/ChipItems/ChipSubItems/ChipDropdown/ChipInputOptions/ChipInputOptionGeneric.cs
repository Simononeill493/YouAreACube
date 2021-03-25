using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipInputOptionGeneric : ChipInputOptionReference
    {
        public string BaseOutput;

        public ChipInputOptionGeneric(ChipTopStandard chipReference, string baseOutput) : base(chipReference)
        {
            BaseOutput = baseOutput;
            OptionType = InputOptionType.Generic;
        }
    }
}
