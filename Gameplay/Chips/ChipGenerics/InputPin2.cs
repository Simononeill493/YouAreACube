using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface InputPin2<TInputType> : IChip
    {
        TInputType ChipInput2 { get; set; }
    }
}
