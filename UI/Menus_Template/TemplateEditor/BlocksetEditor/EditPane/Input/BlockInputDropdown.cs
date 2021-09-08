using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputDropdown : ObservableDropdownMenuItem<BlockInputOption>
    {
        public List<string> InputTypes;
        public BlockInputModel Model;

        public BlockInputDropdown(IHasDrawLayer parent, List<string> inputTypes,BlockInputModel model,Func<string> textProvider) : base(parent,textProvider)
        {
            SetInputTypes(inputTypes);
            Model = model;

            ObservableText.ObservableTextChanged += TextTyped;

            base.ListItemSelected(model.InputOption);
        }

        public void SetInputTypes(List<string> inputTypes)
        {
            InputTypes = inputTypes;
            Editable = inputTypes.Any(t => ChipDropdownUtils.IsTextEntryType(t));
        }

        public override void PopulateItems()
        {
            ClearItems();

            AddItems(BlocksetEditPane.VariableProvider.GetInputsFromVariables(InputTypes));
            AddItems(BlocksetEditPane.Model.GetInputsFromModel(Model, InputTypes));
            AddItems(BlockDropdownUtils.GetDefaultItems(InputTypes));
        }

        protected override void ListItemSelected(BlockInputOption inputOption)
        {
            base.ListItemSelected(inputOption);
            Model.InputOption = inputOption;
        }

        private void TextTyped(string newText)
        {
            Model.InputOption = BlockInputOption.CreateParseable(newText);
        }
    }
}
