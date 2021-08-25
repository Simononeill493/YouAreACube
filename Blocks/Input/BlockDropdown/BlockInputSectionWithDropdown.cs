using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public partial class BlockInputSectionWithDropdown : BlockInputSection
    {
        public override BlockInputOption CurrentlySelected => _dropdown.SelectedItem;
        protected BlockInputDropdown _dropdown;

        public BlockInputSectionWithDropdown(IHasDrawLayer parent, List<string> inputTypes, string inputDisplayName) : base(parent, inputTypes,inputDisplayName)
        {
            _dropdown = BlockDropdownFactory.Create(this, inputTypes);
            _dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentageOffset, true);
            _dropdown.OnSelectedChanged += ItemSelected;
            AddChild(_dropdown);
        }

        protected override void _manuallySetInput(BlockInputOption option)
        {
            _dropdown.ManuallySetItem(option);
        }

        public override void RefreshInputConnections(List<BlockTop> chipsAbove, TemplateVariableSet variables)
        {
            _dropdown.SetItems(_getValidInputsFromVariables(variables.Dict.Values.ToList()));
            _dropdown.AddItems(_getValidInputsFromAbove(chipsAbove));

            foreach (var typeName in InputBaseTypes)
            {
                _dropdown.AddItems(ChipDropdownUtils.GetDefaultItems(typeName));
            }

            _refreshSelectedVariable(variables);
        }

        protected void _refreshSelectedVariable(TemplateVariableSet variables)
        {
            if (CurrentlySelected != null && CurrentlySelected.OptionType == InputOptionType.Variable)
            {
                var variableIndex = int.Parse(CurrentlySelected.ToJSONRep());
                var variableReference = variables.Dict[variableIndex];
                var refreshedInputType = new BlockInputOptionVariable(variableReference);
                _manuallySetInput(refreshedInputType);
            }
        }

        public override void RefreshText() => _dropdown.RefreshText();
    }
}
