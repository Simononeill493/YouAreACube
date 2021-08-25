using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputSectionDynamicType : BlockInputSectionWithDropdown
    {
        public BlockInputSectionDynamicType(IHasDrawLayer parent, string inputDisplayName) : base(parent,new List<string>() { "object" },inputDisplayName)
        {
        }

        public void SetCurrentType(string typeName)
        {
            InputBaseTypes = new List<string>() { typeName };
            _dropdown.SetInputTypes(InputBaseTypes);
        }
    }
}
