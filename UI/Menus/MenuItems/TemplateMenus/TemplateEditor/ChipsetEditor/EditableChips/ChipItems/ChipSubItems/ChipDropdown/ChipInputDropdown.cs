using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipInputDropdown : DropdownMenuItem<ChipInputOption>
    {
        private List<Type> _inputTypes;
        
        public ChipInputDropdown(IHasDrawLayer parent, List<string> inputTypeNames) : base(parent) 
        {
            var isTextEntry = inputTypeNames.All(name => ChipDropdownUtils.IsTextEntryType(name));
            if (isTextEntry)
            {
                _inputTypes = inputTypeNames.Select(n=>TypeUtils.GetTypeByName(n)).ToList();
                OnTextChanged += TextChanged;
                Editable = true;
            }
            else
            {
                Editable = false;
            }
        }

        private void TextChanged(string newText) => ManuallySetItem(new ChipInputOptionParseable(newText, _inputTypes));
    }
}

