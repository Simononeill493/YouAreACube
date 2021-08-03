using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateSelectedMenuInternal : TemplateSelectedMenu
    {
        Action<InternalTemplateSelectionOption, CubeTemplate> _templateSelectedCallback;

        public TemplateSelectedMenuInternal(IHasDrawLayer parentDrawLayer, Action<InternalTemplateSelectionOption, CubeTemplate> templateSelectedCallback) : base(parentDrawLayer)
        {
            SpriteName = "EmptyMenuRectangleSection_Extended";
            _templateSelectedCallback = templateSelectedCallback;

            _addButton("Select", 6, 141, CoordinateMode.ParentPixelOffset, false, (i) => { _templateSelectButtonPressed(); });
        }

        public void _templateSelectButtonPressed()
        {
            var selectedItem = _templateList.Selected;

            _templateSelectedCallback(InternalTemplateSelectionOption.SpecificTemplate, selectedItem);
        }
    }
}
