using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class TemplateVariable
    {
        public int VariableNumber;
        public string VariableName;
        public InGameType VariableType;

        public TemplateVariable(int variableNumber, string variableName, InGameType variableType)
        {
            VariableNumber = variableNumber;
            VariableName = variableName;
            VariableType = variableType;
        }
    }
}
