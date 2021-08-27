using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class BlockInputOptionVariableAccess : BlockInputOption
    {
        public TemplateVariable VariableReference { get; }
        public override string BaseType => "int";

        public BlockInputOptionVariableAccess(TemplateVariable variableReference) : base(InputOptionType.Value)
        {
            VariableReference = variableReference;
        }

        public override string ToJSONRep() => VariableReference.VariableNumber.ToString();
        public override string ToString() => VariableReference.VariableName;
    }
}
