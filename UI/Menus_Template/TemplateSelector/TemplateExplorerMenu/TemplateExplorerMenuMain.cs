using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateExplorerMenuMain: TemplateExplorerMenu
    {
        protected Action<CubeTemplate> _templateSelectedCallback;

        public TemplateExplorerMenuMain(IHasDrawLayer parentDrawLayer, Kernel kernel, Action<CubeTemplate> templateSelectedCallback) : base(parentDrawLayer,kernel)
        {
            _templateSelectedCallback = templateSelectedCallback;

            templateSelectedMenu = new TemplateSelectedMenuMain(this, _templateSelectedAction);
            templateSelectedMenu.SetLocationConfig(65, 0, CoordinateMode.ParentPercentageOffset);
            AddChild(templateSelectedMenu);
        }

        protected void _templateSelectedAction(CubeTemplate template, TemplateSelectedAction selectedActionType)
        {
            if (template != null)
            {
                switch (selectedActionType)
                {
                    case TemplateSelectedAction.Edit:
                        _templateSelectedCallback(template);
                        break;
                    case TemplateSelectedAction.Clone:
                        break;
                }
            }
        }

    }
}
