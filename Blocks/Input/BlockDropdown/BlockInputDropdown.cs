using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class BlockInputDropdown : DropdownMenuItem<BlockInputOption>
    {
        private List<Type> _inputTypes;
        
        public BlockInputDropdown(IHasDrawLayer parent, List<string> inputTypeNames) : base(parent) 
        {
            SetInputTypes(inputTypeNames);
        }

        public void SetInputTypes(List<string> inputTypeNames)
        {
            var isTextEntry = inputTypeNames.All(name => ChipDropdownUtils.IsTextEntryType(name));
            if (isTextEntry)
            {
                _inputTypes = inputTypeNames.Select(n => TypeUtils.GetTypeByDisplayName(n)).ToList();
                OnTextChanged += TextChanged;
                Editable = true;
            }
            else
            {
                Editable = false;
            }
        }

        private void TextChanged(string newText) => ManuallySetItem(new BlockInputOptionParseable(newText, _inputTypes));
    }
}

