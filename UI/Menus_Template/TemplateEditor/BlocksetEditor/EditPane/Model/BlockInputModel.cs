using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputModel
    {
        public BlockInputOption InputOption = BlockInputOption.Undefined;

        public InputOptionType InputOptionType => InputOption.InputOptionType;
        public string StoredValue => InputOption.GetStoredValue();
        public string DisplayValue => InputOption.GetDisplayValue();
    }
}
