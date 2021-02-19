using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateSelectedMenu : SpriteMenuItem
    {
        private Kernel _kernel;
        private BlockTemplate _template;

        public TemplateSelectedMenu(IHasDrawLayer parentDrawLayer,Kernel kernel) : base(parentDrawLayer,"EmptyMenuRectangleSection")
        {
            _kernel = kernel;
        }

    }
}
