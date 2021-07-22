using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{

    class TemplateSelectedMenu : SpriteMenuItem
    {
        private TemplateVersionDictionary _template;
        private ListMenuItem<CubeTemplate> _templateList;

        private TemplateBox _templatePicture;
        private TextMenuItem _templateName;
        private Action<CubeTemplate, TemplateSelectedAction> _templateButtonPressCallback;

        public TemplateSelectedMenu(IHasDrawLayer parentDrawLayer, Action<CubeTemplate,TemplateSelectedAction> templateButtonPressCallback) : base(parentDrawLayer,"EmptyMenuRectangleSection")
        {
            _templateButtonPressCallback = templateButtonPressCallback;

            _templatePicture = new TemplateBox(this, (t) => { });
            _templatePicture.SetLocationConfig(6, 8, CoordinateMode.ParentPixelOffset);
            AddChild(_templatePicture);

            _templateName = new TextMenuItem(this);
            _templateName.SetLocationConfig(12, 65, CoordinateMode.ParentPixelOffset);
            _templateName.MultiplyScale(0.5f);
            AddChild(_templateName);

            _templateList = new ListMenuItem<CubeTemplate>(this,new IntPoint(64,15));
            _templateList.SetLocationConfig(7, 40, CoordinateMode.ParentPixelOffset);
            AddChild(_templateList);


            _addButton("Set Main", 6, 107, CoordinateMode.ParentPixelOffset,false, (i) => { _buttonPressAction(TemplateSelectedAction.SetMain); });
            _addButton("Clone", 6, 124, CoordinateMode.ParentPixelOffset, false, (i) => { _buttonPressAction(TemplateSelectedAction.Clone); });
            _addButton("Edit", 6, 141, CoordinateMode.ParentPixelOffset, false, (i) => { _buttonPressAction(TemplateSelectedAction.Edit); });
        }


        public void SetTemplate(TemplateVersionDictionary template)
        {
            _template = template;
            _templatePicture.SetTemplate(template);
            _templatePicture.UpdateDimensionsCascade(ActualLocation, GetCurrentSize());

            _templateName.Text = _template.Name;

            _templateList.SetItems(template.Versions);

            var mainItem = _templateList.Items.First(i => i.Item == template.Main);
            mainItem.TrySelectItem();
        }
        private void _buttonPressAction(TemplateSelectedAction selectedAction)
        {
            if(_template!=null)
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
