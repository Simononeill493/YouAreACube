using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class BlockInputSectionVariableAccess : BlockInputSectionWithDropdown
    { 
        public BlockInputSectionVariableAccess(IHasDrawLayer parent,string inputDisplayName,Action<BlockInputOption> variableTypeChangedCallback) : base(parent, new List<string>(),inputDisplayName)
        {
            _dropdown.OnSelectedChanged += variableTypeChangedCallback;
        }

        public override void RefreshInputConnections(List<BlockTop> chipsAbove, TemplateVariableSet variables)
        {
            _dropdown.SetItems(_getAllValidVariables(variables));

            _refreshSelectedVariable(variables);
        }

        protected List<BlockInputOption> _getAllValidVariables(TemplateVariableSet variables)
        {
            return variables.Dict.Values.Select(v => new BlockInputOptionVariableAccess(v)).Cast<BlockInputOption>().ToList();
        }
    }
}
