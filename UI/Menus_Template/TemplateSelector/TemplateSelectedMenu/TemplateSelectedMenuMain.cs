using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateSelectedMenuMain : TemplateSelectedMenu
    {
        private Action<CubeTemplate, TemplateSelectedAction> _templateButtonPressCallback;

        public TemplateSelectedMenuMain(IHasDrawLayer parentDrawLayer, Action<CubeTemplate, TemplateSelectedAction> templateButtonPressCallback) : base(parentDrawLayer)
        {
            _templateButtonPressCallback = templateButtonPressCallback;

            _addButton("Set Main", 6, 107, CoordinateMode.ParentPixelOffset, false, (i) => { _buttonPressAction(TemplateSelectedAction.SetMain); });
            _addButton("Clone", 6, 124, CoordinateMode.ParentPixelOffset, false, (i) => { _buttonPressAction(TemplateSelectedAction.Clone); });
            _addButton("Edit", 6, 141, CoordinateMode.ParentPixelOffset, false, (i) => { _buttonPressAction(TemplateSelectedAction.Edit); });
        }

        protected void _buttonPressAction(TemplateSelectedAction selectedAction)
        {
            if (_template != null)
            {
                var version = _template[_templateList.Selected.Version];
                if (selectedAction == TemplateSelectedAction.SetMain)
                {
                    _template.Main = version;
                }

                _templateButtonPressCallback(version, selectedAction);
            }
        }
    }
}
