using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputDropdown_2 : ObservableDropdownMenuItem<BlockInputOption_2>
    {
        public List<string> InputTypes;
        public BlockInputModel Model;

        public BlockInputDropdown_2(IHasDrawLayer parent, List<string> inputTypes,BlockInputModel model,Func<string> textProvider) : base(parent,textProvider)
        {
            SetInputTypes(inputTypes);
            Model = model;

            ObservableText.ObservableTextChanged += TextTyped;
        }

        public void SetInputTypes(List<string> inputTypes)
        {
            InputTypes = inputTypes;
            Editable = inputTypes.Any(t => ChipDropdownUtils.IsTextEntryType(t));
        }

        public override void PopulateItems()
        {
            ClearItems();

            AddItems(BlocksetEditPane_2.VariableProvider.GetInputsFromVariables(InputTypes));
            AddItems(BlocksetEditPane_2.Model.GetInputsFromModel(Model, InputTypes));
            AddItems(BlockDropdownUtils.GetDefaultItems(InputTypes));
        }

        protected override void ListItemSelected(BlockInputOption_2 inputOption)
        {
            base.ListItemSelected(inputOption);
            Model.SetInputOption = inputOption;
        }

        private void TextTyped(string newText)
        {
            Model.SetInputOption = BlockInputOption_2.CreateParseable(newText);
        }
    }
}
