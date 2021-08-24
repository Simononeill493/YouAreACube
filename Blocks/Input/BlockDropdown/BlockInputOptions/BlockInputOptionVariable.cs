using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class BlockInputOptionVariable : BlockInputOption
    {
        public TemplateVariable VariableReference { get; }
        public override string BaseType => VariableReference.VariableType.Name;

        public BlockInputOptionVariable(TemplateVariable variableReference) : base(InputOptionType.Variable)
        {
            VariableReference = variableReference;
        }

        public override string ToJSONRep() => VariableReference.VariableNumber.ToString();
        public override string ToString() => VariableReference.VariableName;
    }
}
