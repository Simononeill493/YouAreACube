using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputModel
    {
        public BlockInputOption_2 SetInputOption = BlockInputOption_2.Undefined;

        public InputOptionType InputOptionType => SetInputOption.InputOptionType;
        public string StoredValue => SetInputOption.GetStoredValue();
        public string DisplayValue => SetInputOption.GetDisplayValue();
    }
}
