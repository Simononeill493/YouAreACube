using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipSectionFactory
    {
        public static ChipItemMiddleSection Create(IHasDrawLayer parentDrawLayer,string dataType)
        {
            var section = new ChipItemMiddleSection(parentDrawLayer, dataType);
            var dropdown = ChipDropdownFactory.Create(section, dataType);

            dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentageOffset, true);
            section.AddChild(dropdown);
            section.Dropdown = dropdown;

            return section;
        }
    }
}
