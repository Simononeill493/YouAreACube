using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipInputDropdown : DropdownMenuItem<ChipInputOption>
    {
        private Type _inputType;
        
        public ChipInputDropdown(IHasDrawLayer parent,string inputTypeName) : base(parent) 
        {
            var isTextEntry = ChipDropdownUtils.IsTextEntryType(inputTypeName);

            if (isTextEntry)
            {
                _inputType = TypeUtils.GetTypeByName(inputTypeName);
                OnTextChanged += TextChanged;
                Editable = true;
            }
            else
            {
                Editable = false;
            }
        }

        private void TextChanged(string newText)
        {
            var newValue = new ChipInputOptionParseable(newText,_inputType);
            ManuallySetItem(newValue);
        }
    }
}

