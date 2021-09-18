using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateSelectedMenuInternal : TemplateSelectedMenu
    {
        public TemplateSelectedMenuInternal(IHasDrawLayer parentDrawLayer, Action<CubeTemplate> templateSelectedCallback) : base(parentDrawLayer)
        {
            SpriteName = MenuSprites.TemplateListMenuSection_Extended;

            _addButton("Select", 6, 141, CoordinateMode.ParentPixel, false, (i) => { templateSelectedCallback(_templateList.Selected); });
        }

        protected override void _setTemplateListToThisTemplateDict(TemplateVersionDictionary template)
        {
            var list = new List<CubeTemplate>();
            list.Add(new CubeTemplateMainPlaceholder(template.Name));
            list.AddRange(template.Versions);

            _templateList.SetItems(list);
        }
    }
}
