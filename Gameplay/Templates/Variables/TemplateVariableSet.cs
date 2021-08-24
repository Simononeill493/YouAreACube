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

    }
}
