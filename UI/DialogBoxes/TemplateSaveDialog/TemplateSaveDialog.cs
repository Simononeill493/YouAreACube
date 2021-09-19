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

        private Action<TemplateSaveDialogOption, string> _saveTemplateCallback;
        private string _saveTextName;

        private string _selectedName;

        public TemplateSaveDialog(IHasDrawLayer parentDrawLayer, ScreenItem container,int newVersionNumber,string currentName,Action<TemplateSaveDialogOption,string> saveTemplateCallback) : base(parentDrawLayer, container, MenuSprites.MediumMenuRectangle)
        {
            _selectedName = currentName;
            _saveTemplateCallback = saveTemplateCallback;

            _addTextItem(()=>_saveTextName, 50, 15, CoordinateMode.ParentPercentage, true);
            _versionText = _addStaticTextItem("V" + newVersionNumber + ":", 10, 30, CoordinateMode.ParentPercentage, true);
            _addTextBox(()=>_selectedName,(s)=> { _selectedName = s; }, 50, 30, CoordinateMode.ParentPercentage, true, editable: true, maxTextLength: 12);
            _addButton("Save", 30, 80, CoordinateMode.ParentPercentage, true, (i) => _saveButtonPressed());
            _addButton("Cancel", 70, 80, CoordinateMode.ParentPercentage, true, (i) => Close());

            var saveTypeRadioButtons = new RadioButtonsMenuItem<TemplateSaveDialogOption>(this);
            saveTypeRadioButtons.AddOption(TemplateSaveDialogOption.SaveAsNewTemplate,"As new template");
            saveTypeRadioButtons.AddOption(TemplateSaveDialogOption.SaveAsNewVersion, "As new version");
            saveTypeRadioButtons.OnItemSelected += _dialogOptionSelected;
            saveTypeRadioButtons.SetLocationConfig(10, 50, CoordinateMode.ParentPercentage, false);
            AddChild(saveTypeRadioButtons);

            saveTypeRadioButtons.SelectRadioButton(1);
        }

        private void _saveButtonPressed()
        {
            _saveTemplateCallback(SelectedDialogOption, _selectedName);
            Close();
        }

        private void _dialogOptionSelected(TemplateSaveDialogOption option)
        {
            SelectedDialogOption = option;

            switch (option)
            {
                case TemplateSaveDialogOption.SaveAsNewTemplate:
                    _versionText.Visible = false;
                    _saveTextName = "Template name";
                    break;
                case TemplateSaveDialogOption.SaveAsNewVersion:
                    _versionText.Visible = true;
                    _saveTextName = "Version name";
                    break;
            }
        }
    }
}
