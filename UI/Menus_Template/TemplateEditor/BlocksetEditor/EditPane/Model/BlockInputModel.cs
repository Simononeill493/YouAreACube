using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputModel
    {
        public string DisplayValue => InputOption.GetDisplayValue();
        public string StoredType => InputOption.GetStoredType();

        public BlockInputOption InputOption = BlockInputOption.Undefined;

        public BlockInputModel()
        {
            InputOption = BlockInputOption.Undefined;
        }
    }
}
