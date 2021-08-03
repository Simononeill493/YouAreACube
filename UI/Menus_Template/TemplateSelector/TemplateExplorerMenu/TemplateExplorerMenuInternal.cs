using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateExplorerMenuInternal :TemplateExplorerMenu
    {
        public TemplateExplorerMenuInternal(IHasDrawLayer parentDrawLayer, Kernel kernel, Action<CubeTemplate> templateSelectedCallback) : base(parentDrawLayer, kernel)
        {
            templateSelectedMenu = new TemplateSelectedMenuInternal(this, templateSelectedCallback);
            templateSelectedMenu.SetLocationConfig(65, 0, CoordinateMode.ParentPercentageOffset);
            AddChild(templateSelectedMenu);
        }
    }
}
