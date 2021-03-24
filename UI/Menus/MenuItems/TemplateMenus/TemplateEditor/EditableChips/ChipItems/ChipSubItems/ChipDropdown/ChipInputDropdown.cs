using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipInputDropdown : DropdownMenuItem<ChipInputOption>
    {
        public ChipInputDropdown(IHasDrawLayer parent) : base(parent) { }
    }
}

