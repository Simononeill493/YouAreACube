using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public enum TemplateSelectedAction
    {
        Edit,
        Clone,
        SetMain
    }

    class TemplateSelectedMenu : SpriteMenuItem
    {
        private TemplateVersionList _template;
        private int _selectedVersion;

        private TemplateBox _templatePicture;
        private TextMenuItem _templateName;

        private Action<BlockTemplate, TemplateSelectedAction> _templateButtonPressCallback;

        public TemplateSelectedMenu(IHasDrawLayer parentDrawLayer, Action<BlockTemplate,TemplateSelectedAction> templateButtonPressCallback) : base(parentDrawLayer,"EmptyMenuRectangleSection")
        {
            _templateButtonPressCallback = templateButtonPressCallback;

            _templatePicture = new TemplateBox(this, (t) => { });
            _templatePicture.SetLocationConfig(6, 8, CoordinateMode.ParentPixelOffset);
            AddChild(_templatePicture);

            _templateName = new TextMenuItem(this);
            _templateName.SetLocationConfig(12, 65, CoordinateMode.ParentPixelOffset);
            _templateName.MultiplyScale(0.5f);
            AddChild(_templateName);

            var setMainButton = new ButtonMenuItem(this, "Set Main");
            setMainButton.SetLocationConfig(6, 107, CoordinateMode.ParentPixelOffset);
            setMainButton.OnMouseReleased += (i) => { _buttonPressAction(TemplateSelectedAction.SetMain); };
            AddChild(setMainButton);

            var cloneButton = new ButtonMenuItem(this, "Clone");
            cloneButton.SetLocationConfig(6, 124, CoordinateMode.ParentPixelOffset);
            cloneButton.OnMouseReleased += (i) => { _buttonPressAction(TemplateSelectedAction.Clone); };
            AddChild(cloneButton);

            var editButton = new ButtonMenuItem(this, "Edit");
            editButton.SetLocationConfig(6, 141, CoordinateMode.ParentPixelOffset);
            editButton.OnMouseReleased += (i) => { _buttonPressAction(TemplateSelectedAction.Edit); };
            AddChild(editButton);

            var list = new ListMenuItem<string>(this,new Point(64,15));
            list.AddItems(new List<string>() { "Test1", "Test2", "Test3" });
            list.SetLocationConfig(7, 40, CoordinateMode.ParentPixelOffset);
            AddChild(list);
        }

        private void _buttonPressAction(TemplateSelectedAction selectedAction)
        {
            if(_template!=null)
            {
                var version = _template[_selectedVersion];
                _templateButtonPressCallback(version, selectedAction);
            }
        }

        public void SetTemplate(TemplateVersionList template)
        {
            _template = template;
            _templatePicture.SetTemplate(template);
            _templatePicture.UpdateDimensionsCascade(ActualLocation, GetCurrentSize());

            _templateName.Text = _template.Name;
        }
    }
}
