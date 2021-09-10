using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputDropdownMetaVariable : BlockInputDropdown
    {
        public BlockInputDropdownMetaVariable(IHasDrawLayer parent, BlockInputModel model, Func<string> textProvider) : base(parent, new List<string>(), model, textProvider)
        {
            if(model.InputOption.InputOptionType == InputOptionType.Value)
            {
                model.InputOption.InputOptionType = InputOptionType.MetaVariable;
            }
        }

        public override void PopulateItems()
        {
            ClearItems();
            AddItems(BlocksetEditPane.VariableProvider.GetMetaInputsFromVariables());
        }

        public List<string> GetTypesOfSelectedVariable()
        {
            if(Model.InputOption.InputOptionType != InputOptionType.MetaVariable)
            {
                return new List<string>();
            }

            var variable = BlocksetEditPane.VariableProvider.GetVariable((int)Model.InputOption.Value);
            return new List<string>() { variable.VariableType.Name };
        }
    }
}
