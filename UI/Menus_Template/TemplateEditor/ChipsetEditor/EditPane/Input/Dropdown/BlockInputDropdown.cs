using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputDropdown : DropdownMenuItem<BlockInputOption>
    {
        public List<string> InputTypes => _inputTypesProvider();
        public Func<List<string>> _inputTypesProvider;

        public override bool Editable { get => InputTypes.Any(t => InGameTypeUtils.IsTextEntryType(t)); set => throw new NotImplementedException(); }

        public BlockInputModel Model;

        public BlockInputDropdown(IHasDrawLayer parent, List<string> inputTypes, BlockInputModel model, Func<string> textProvider) : base(parent, () => model.InputOption, (o) => { model.InputOption = o; },()=> null)
        {
            SetStaticInputTypes(inputTypes);
            Model = model;

            base.ListItemSelected(model.InputOption);

            OnTextTyped += TextTyped;
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
            _list.ClearItems();

            _list.AddItems(BlocksetEditPane.VariableProvider.GetInputsFromVariables(InputTypes));
            _list.AddItems(BlocksetEditPane.Model.GetInputsFromModel(Model, InputTypes));
            _list.AddItems(BlockDropdownUtils.GetDefaultItems(InputTypes));
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
