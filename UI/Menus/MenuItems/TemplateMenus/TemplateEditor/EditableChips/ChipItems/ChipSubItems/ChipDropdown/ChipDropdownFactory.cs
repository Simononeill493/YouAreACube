using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipDropdownFactory
    {
        public static ChipInputDropdown Create(ChipInputSection section,string inputType)
        {
            var dropdown = new ChipInputDropdown(section,inputType);
            dropdown.SetItems(ChipDropdownUtils.GetDefaultItems(inputType));

            return dropdown;
        }
    }
}
