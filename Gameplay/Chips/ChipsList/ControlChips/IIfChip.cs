using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    interface IIfChip
    {
        Chipset Yes { get; set; }
        Chipset No { get; set; }
    }
}
