using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public enum TemplateSelectedAction
    {
        Edit
    }

    class TemplateSelectedMenu : SpriteMenuItem
    {
        private BlockTemplate _template;

        private TemplateBox _templatePicture;
        private TextMenuItem _templateName;
        private TextBoxMenuItem _editButton;

        private Action<BlockTemplate, TemplateSelectedAction> _doTemplateSelectedAction;

        public TemplateSelectedMenu(IHasDrawLayer parentDrawLayer, Action<BlockTemplate,TemplateSelectedAction> doTemplateSelectedAction) : base(parentDrawLayer,"EmptyMenuRectangleSection")
        {
            _doTemplateSelectedAction = doTemplateSelectedAction;

            _templatePicture = new TemplateBox(this, (t) => { });
            _templatePicture.SetLocationConfig(10, 5, CoordinateMode.ParentPercentageOffset);
            AddChild(_templatePicture);

            _templateName = new TextMenuItem(this);
            _templateName.SetLocationConfig(10, 20, CoordinateMode.ParentPercentageOffset);
            _templateName.HalfScaled = true;
            AddChild(_templateName);

            _editButton = new ButtonMenuItem(this, "Edit...");
            _editButton.SetLocationConfig(50, 90, CoordinateMode.ParentPercentageOffset, true);
            _editButton.OnClick += () => { doTemplateSelectedAction(_template, TemplateSelectedAction.Edit); };
            AddChild(_editButton);
        }

        public void SetTemplate(BlockTemplate template)
        {
            _template = template;
            _templatePicture.SetTemplate(template);
            _templatePicture.UpdateThisAndChildLocations(ActualLocation, GetSize());

            _templateName.Text = _template.Name;
        }
    }
}
