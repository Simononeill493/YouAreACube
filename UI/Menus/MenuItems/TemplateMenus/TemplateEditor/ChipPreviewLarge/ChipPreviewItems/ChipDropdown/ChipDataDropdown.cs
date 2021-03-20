using System;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipDataDropdown : DropdownMenuItem<ChipInputPinDropdownSelection>
    {
        public Type BaseType;
        public string DataType;

        public ChipDataDropdown(IHasDrawLayer parent, string dataType) : base(parent)
        {
            DataType = dataType;
        }

        public void ResetToDefaults()
        {
            ChipSectionFactory.SetDefaultItems(this, DataType);
        }
    }
}
