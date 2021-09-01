using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ObservableDropdownMenuItem : DropdownMenuItem<object>
    {
        public ObservableDropdownMenuItem(IHasDrawLayer parent) : base(parent)
        {

        }
    }
}
