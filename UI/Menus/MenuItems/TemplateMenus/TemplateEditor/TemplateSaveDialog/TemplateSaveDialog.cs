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

        public TemplateSaveDialog(IHasDrawLayer parentDrawLayer, MenuItem container,int newVersionNumber,Action<TemplateSaveDialogOption,string> saveTemplateCallback) : base(parentDrawLayer, container, "EmptyMenuRectangleMedium")
        {
            _saveTemplateCallback = saveTemplateCallback;

            _nameText = new TextMenuItem(this, "");
            _versionText = new TextMenuItem(this, "V" + newVersionNumber + ":");
            _nameTextBox = new TextBoxMenuItem(this, "") { Editable = true, MaxTextLength=12 };
            var saveTypeRadioButtons = new RadioButtonsMenuItem<TemplateSaveDialogOption>(this);
            var saveButton = new ButtonMenuItem(this, "Save");
            var cancelButton = new ButtonMenuItem(this, "Cancel");

            saveTypeRadioButtons.AddOption(TemplateSaveDialogOption.SaveAsNewTemplate,"As new template");
            saveTypeRadioButtons.AddOption(TemplateSaveDialogOption.SaveAsNewVersion, "As new version");
            saveTypeRadioButtons.OnItemSelected += _dialogOptionSelected;
            saveButton.OnMouseReleased += (i) =>_saveButtonPressed();
            cancelButton.OnMouseReleased += (i) => Close();

            _nameText.SetLocationConfig(50, 15, CoordinateMode.ParentPercentageOffset, true);
            _versionText.SetLocationConfig(10, 30, CoordinateMode.ParentPercentageOffset, true);
            _nameTextBox.SetLocationConfig(50, 30, CoordinateMode.ParentPercentageOffset, true);
            saveTypeRadioButtons.SetLocationConfig(10, 50, CoordinateMode.ParentPercentageOffset, false);
            saveButton.SetLocationConfig(30, 80, CoordinateMode.ParentPercentageOffset, true);
            cancelButton.SetLocationConfig(70, 80, CoordinateMode.ParentPercentageOffset, true);

            AddChild(_nameText);
            AddChild(_versionText);
            AddChild(saveButton);
            AddChild(cancelButton);
            AddChild(_nameTextBox);
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
