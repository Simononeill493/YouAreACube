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
        public static ChipInputDropdown Create(ChipInputSection section,List<string> inputTypes)
        {
            var dropdown = new ChipInputDropdown(section,inputTypes);

            var options = ChipDropdownUtils.GetDefaultItems(inputTypes);
            dropdown.SetItems(options);

            return dropdown;
        }
    }
}
