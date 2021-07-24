using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class BlockDropdownFactory
    {
        public static BlockInputDropdown Create(BlockInputSection section,List<string> inputTypes)
        {
            var dropdown = new BlockInputDropdown(section,inputTypes);

            var options = ChipDropdownUtils.GetDefaultItems(inputTypes);
            dropdown.SetItems(options);

            return dropdown;
        }
    }
}
