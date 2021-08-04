using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class CubeTemplateMainPlaceholder : CubeTemplate
    {
        public CubeTemplateMainPlaceholder(string name) : base(name) { }

        public override CubeTemplate GetRuntimeTemplate()
        {
            var versions = Templates.Database[Name];
            return versions.Main;
        }

        public override string ToString() => Name + "|Main";

    }
}
