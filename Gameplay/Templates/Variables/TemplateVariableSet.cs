using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class TemplateVariableSet 
    {
        public Dictionary<int, TemplateVariable> Dict;

        public TemplateVariableSet()
        {
            Dict = new Dictionary<int, TemplateVariable>();
        }

        public virtual Dictionary<int, object> GenerateVariables()
        {
            if (!Dict.Any())
            {
                return null;
            }

            var varDict = new Dictionary<int, object>();
            foreach(var kvp in Dict)
            {
                varDict[kvp.Key] = kvp.Value.VariableType.DefaultValue;
            }

            return varDict;
        }
    }
}
