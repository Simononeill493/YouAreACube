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
        public static ChipInputDropdown Create(ChipMiddleSection section,string dataType)
        {
            var dropdown = new ChipInputDropdown(section, dataType);
            ChipDropdownUtils.SetDefaultItems(dropdown, dataType);

            if (ChipDropdownUtils.IsTextEntryType(dataType))
            {
                dropdown.Editable = true;
            }

            dropdown.BaseType = ChipDropdownUtils.GetTypeOfDataType(dataType);
            return dropdown;
        }
    }
}
