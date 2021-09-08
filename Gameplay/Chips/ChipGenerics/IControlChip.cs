using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface IControlChip
    {
        List<(string, Chipset)> GetSubChipsets();
        void SetSubChipsets(List<(string, Chipset)> subChipsets);

        List<Chipset> GetSubChipsetsCascade();
    }
}
