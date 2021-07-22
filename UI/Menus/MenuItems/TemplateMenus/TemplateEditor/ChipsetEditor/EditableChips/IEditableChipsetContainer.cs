using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface IEditableChipsetContainer
    {
        void AddChipset(Blockset chipset);
        void RemoveChipset(Blockset chipset);
    }
}
