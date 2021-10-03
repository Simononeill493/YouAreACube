using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{

    abstract class TemplateSelectedMenu : SpriteScreenItem
    {
        protected TemplateVersionDictionary _template;
        protected ListMenuItem<CubeTemplate> _templateList;

        protected TemplateBox _templatePicture;
        protected TextScreenItem _templateName;

        public TemplateSelectedMenu(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer,MenuSprites.TemplateListMenuSection)
        {
            _templatePicture = new TemplateBox(this, (t) => { });
            _templatePicture.SetLocationConfig(6, 8, CoordinateMode.ParentPixel);
            AddChild(_templatePicture);

            _templateName = new TextScreenItem(this,()=>_template?.Name);
            _templateName.SetLocationConfig(12, 65, CoordinateMode.ParentPixel);
            _templateName.MultiplyScale(0.5f);
            AddChild(_templateName);

            _templateList = new ListMenuItem<CubeTemplate>(this,new IntPoint(64,15));
            _templateList.SetLocationConfig(7, 40, CoordinateMode.ParentPixel);
            AddChild(_templateList);
        }

        public void SetTemplate(TemplateVersionDictionary template)
        {
            _template = template;
            _templatePicture.SetTemplate(template);
            _templatePicture.UpdateLocationCascade(ActualLocation, GetCurrentSize());

            _setTemplateListToThisTemplateDict(template);

            var mainItem = _templateList.Items.First(i => i.Item == template.Main);
            mainItem.TrySelectItem();
        }

        protected virtual void _setTemplateListToThisTemplateDict(TemplateVersionDictionary template)
        {
            _templateList.SetItems(template.Versions);
        }
    }
}
