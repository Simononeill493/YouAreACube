using System;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipInputDropdown : DropdownMenuItem<ChipInputOption>
    {
        public Type BaseType;
        public string DataType;

        public ChipInputDropdown(IHasDrawLayer parent, string dataType) : base(parent)
        {
            DataType = dataType;
        }

        public void ResetToDefaults()
        {
            ChipDropdownUtils.SetDefaultItems(this, DataType);
        }
    }
}
