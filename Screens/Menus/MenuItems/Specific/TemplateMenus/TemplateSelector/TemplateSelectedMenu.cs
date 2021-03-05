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

        public TemplateSelectedMenu(IHasDrawLayer parentDrawLayer, Action<BlockTemplate,TemplateSelectedAction> templateSelectedAction) : base(parentDrawLayer,"EmptyMenuRectangleSection")
        {
            _templatePicture = new TemplateBox(this, (t) => { });
            _templatePicture.SetLocationConfig(10, 5, CoordinateMode.ParentPercentageOffset);
            AddChild(_templatePicture);

            _templateName = new TextMenuItem(this);
            _templateName.SetLocationConfig(10, 20, CoordinateMode.ParentPercentageOffset);
            _templateName.MultiplyScale(0.5f);
            AddChild(_templateName);

            _editButton = new ButtonMenuItem(this, "Edit");
            _editButton.SetLocationConfig(50, 90, CoordinateMode.ParentPercentageOffset, true);
            _editButton.OnMouseReleased += (i) => { templateSelectedAction(_template, TemplateSelectedAction.Edit); };
            AddChild(_editButton);
        }

        public void SetTemplate(BlockTemplate template)
        {
            _template = template;
            _templatePicture.SetTemplate(template);
            _templatePicture.UpdateDimensionsCascade(ActualLocation, GetCurrentSize());

            _templateName.Text = _template.Name;
        }
    }
}
