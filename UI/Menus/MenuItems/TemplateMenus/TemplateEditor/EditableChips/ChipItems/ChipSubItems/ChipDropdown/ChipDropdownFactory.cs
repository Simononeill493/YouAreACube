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
            var dropdown = new ChipInputDropdown(section);
            dropdown.SetItems(ChipDropdownUtils.GetDefaultItems(inputType));
            dropdown.Editable = ChipDropdownUtils.IsTextEntryType(inputType);

            return dropdown;
        }
    }
}
