using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class TemplateSaveDialog: DialogBoxMenuItem
    {
        public TemplateSaveDialogOption SelectedDialogOption;

        private TextMenuItem _versionText;
        private TextMenuItem _nameText;
        private TextBoxMenuItem _nameTextBox;

        private Action<TemplateSaveDialogOption, string> _saveTemplateCallback;

        public TemplateSaveDialog(IHasDrawLayer parentDrawLayer, MenuItem container,int newVersionNumber,string currentName,Action<TemplateSaveDialogOption,string> saveTemplateCallback) : base(parentDrawLayer, container, BuiltInMenuSprites.MediumMenuRectangle)
        {
            _saveTemplateCallback = saveTemplateCallback;

            _nameText = _addTextItem("", 50, 15, CoordinateMode.ParentPercentageOffset, true);
            _versionText = _addTextItem("V" + newVersionNumber + ":", 10, 30, CoordinateMode.ParentPercentageOffset, true);
            _nameTextBox = _addTextBox(currentName, 50, 30, CoordinateMode.ParentPercentageOffset, true, editable: true, maxTextLength: 12);
            _addButton("Save", 30, 80, CoordinateMode.ParentPercentageOffset, true, (i) => _saveButtonPressed());
            _addButton("Cancel", 70, 80, CoordinateMode.ParentPercentageOffset, true, (i) => Close());

            var saveTypeRadioButtons = new RadioButtonsMenuItem<TemplateSaveDialogOption>(this);
            saveTypeRadioButtons.AddOption(TemplateSaveDialogOption.SaveAsNewTemplate,"As new template");
            saveTypeRadioButtons.AddOption(TemplateSaveDialogOption.SaveAsNewVersion, "As new version");
            saveTypeRadioButtons.OnItemSelected += _dialogOptionSelected;
            saveTypeRadioButtons.SetLocationConfig(10, 50, CoordinateMode.ParentPercentageOffset, false);
            AddChild(saveTypeRadioButtons);

            saveTypeRadioButtons.SelectRadioButton(1);
        }

        private void _saveButtonPressed()
        {
            _saveTemplateCallback(SelectedDialogOption, _nameTextBox.Text);
            Close();
        }

        private void _dialogOptionSelected(TemplateSaveDialogOption option)
        {
            SelectedDialogOption = option;

            switch (option)
            {
                case TemplateSaveDialogOption.SaveAsNewTemplate:
                    _versionText.Visible = false;
                    _nameText.Text = "Template name";
                    break;
                case TemplateSaveDialogOption.SaveAsNewVersion:
                    _versionText.Visible = true;
                    _nameText.Text = "Version name";
                    break;
            }
        }
    }
}
