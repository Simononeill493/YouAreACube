using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateEditMenu : SpriteMenuItem
    {
        private Kernel _kernel;
        private BlockTemplate _template;

        public TemplateEditMenu(Kernel kernel, BlockTemplate template) : base("EmptyMenuRectangleFull")
        {
            _kernel = kernel;
            _template = template;
        }
    }
}
