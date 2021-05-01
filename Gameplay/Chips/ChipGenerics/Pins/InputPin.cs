using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface InputPin<TInputType> : IChip
    {
        TInputType ChipInput1 { get;  set; }
    }
}
