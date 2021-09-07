using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputModel
    {
        public BlockInputOption_2 InputOption = BlockInputOption_2.Undefined;

        public InputOptionType InputOptionType => InputOption.InputOptionType;
        public string StoredValue => InputOption.GetStoredValue();
        public string DisplayValue => InputOption.GetDisplayValue();
    }
}
