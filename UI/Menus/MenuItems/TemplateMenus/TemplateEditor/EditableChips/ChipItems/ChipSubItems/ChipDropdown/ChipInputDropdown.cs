using System;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipInputDropdown : DropdownMenuItem<ChipInputOption>
    {
        public ChipInputDropdown(IHasDrawLayer parent) : base(parent) { }

        public void ResetToDefaults(string inputType)
        {
            ChipDropdownUtils.SetDefaultItems(this, inputType);
        }
    }
}
