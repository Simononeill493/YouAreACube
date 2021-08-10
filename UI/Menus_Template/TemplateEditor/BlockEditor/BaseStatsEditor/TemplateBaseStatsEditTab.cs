using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateBaseStatsEditTab :SpriteMenuItem
    {
        public string CurrentName => _nameTextbox.Text;

        private TextBoxMenuItem _nameTextbox;
        private TextBoxMenuItem _healthTextBox;
        private TextBoxMenuItem _energyTextBox;
        private TextBoxMenuItem _speedTextBox;
        private CheckBoxMenuItem _activeCheckBox;

        public TemplateBaseStatsEditTab(IHasDrawLayer parent,CubeTemplate baseTemplate) : base(parent, BuiltInMenuSprites.LargeMenuRectangle)
        {
            _addTextItem("Name:", 20, 10, CoordinateMode.ParentPercentageOffset, true);
            _addTextItem("Health:", 20, 25, CoordinateMode.ParentPercentageOffset, true);
            _addTextItem("Energy:", 20, 40, CoordinateMode.ParentPercentageOffset, true);
            _addTextItem("Speed:", 20, 55, CoordinateMode.ParentPercentageOffset, true);
            _addTextItem("Active:", 20, 80, CoordinateMode.ParentPercentageOffset, true);

            _nameTextbox = _addTextBox("", 60, 10, CoordinateMode.ParentPercentageOffset, true, editable: true, maxTextLength: 15); ;
            _healthTextBox = _addTextBox("", 60, 25, CoordinateMode.ParentPercentageOffset, true, editable: true, maxTextLength: 4);
            _energyTextBox = _addTextBox("", 60, 40, CoordinateMode.ParentPercentageOffset, true, editable: true, maxTextLength: 4);
            _speedTextBox = _addTextBox("", 60, 55, CoordinateMode.ParentPercentageOffset, true, editable: true, maxTextLength: 6);

            _activeCheckBox = new CheckBoxMenuItem(this);
            _activeCheckBox.SetLocationConfig(35, 80, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_activeCheckBox);

            LoadFieldsForEditing(baseTemplate);
        }

        public void LoadFieldsForEditing(CubeTemplate template)
        {
            _nameTextbox.SetText(template.Name);
            _healthTextBox.SetText(template.MaxHealth.ToString());
            _energyTextBox.SetText(template.MaxEnergy.ToString());
            _speedTextBox.SetText(template.Speed.ToString());
            _activeCheckBox.Set(template.Active);
        }

        public void AddEditedFieldsToTemplate(CubeTemplate template)
        {
            template.Name = _nameTextbox.Text;
            template.MaxHealth = int.Parse(_healthTextBox.Text);
            template.MaxEnergy = int.Parse(_energyTextBox.Text);
            template.Speed = int.Parse(_speedTextBox.Text);
            template.Active = _activeCheckBox.Checked;
        }


    }
}
