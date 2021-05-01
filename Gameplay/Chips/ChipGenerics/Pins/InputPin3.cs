using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface InputPin3<TInputType> : IChip
    {
        TInputType ChipInput3 { get; set; }
    }
}
