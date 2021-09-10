using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputDropdown : ObservableDropdownMenuItem<BlockInputOption>
    {
        public List<string> InputTypes => _inputTypesProvider();
        public Func<List<string>> _inputTypesProvider;

        public override bool Editable { get => InputTypes.Any(t => InGameTypeUtils.IsTextEntryType(t)); set => throw new NotImplementedException(); }

        public BlockInputModel Model;

        public BlockInputDropdown(IHasDrawLayer parent, List<string> inputTypes, BlockInputModel model,Func<string> textProvider) : base(parent,textProvider)
        {
            SetStaticInputTypes(inputTypes);
            Model = model;

            ObservableText.ObservableTextChanged += TextTyped;

            base.ListItemSelected(model.InputOption);
        }

        public void SetInputTypeProvider(Func<List<string>> inputTypesProvider)
        {
            _inputTypesProvider = inputTypesProvider;
        }

        public void SetStaticInputTypes(List<string> inputTypes)
        {
            _inputTypesProvider = () => inputTypes;
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
            var attemptedParsedValue = InGameTypeUtils.ParseType(InputTypes, newText);
            if(attemptedParsedValue!=null)
            {
                Model.InputOption = BlockInputOption.CreateValue(attemptedParsedValue);
            }
            else
            {
                Model.InputOption = BlockInputOption.CreateUnparseable(newText);
            }
        }
    }
}
