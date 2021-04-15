using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipSaveDialog: DialogBoxMenuItem
    {
        private TextMenuItem _versionText;

        public ChipSaveDialog(IHasDrawLayer parentDrawLayer, TemplateEditMenu container,int newVersionNumber) : base(parentDrawLayer, container, "EmptyMenuRectangleMedium")
        {
            _versionText = new TextMenuItem(this, "V" + newVersionNumber + ":");
            var nameTextBox = new TextBoxMenuItem(this, "") { Editable = true };
            var saveTypeRadioButtons = new RadioButtonsMenuItem<ChipSaveDialogOption>(this);
            var saveButton = new ButtonMenuItem(this, "Save");
            var cancelButton = new ButtonMenuItem(this, "Cancel");

            saveTypeRadioButtons.AddOption(ChipSaveDialogOption.SaveAsNewTemplate,"As new template");
            saveTypeRadioButtons.AddOption(ChipSaveDialogOption.SaveAsNewVersion, "As new version");
            saveTypeRadioButtons.OnItemSelected += _dialogOptionSelected;
            cancelButton.OnMouseReleased += (i) => Close();

            _versionText.SetLocationConfig(10, 30, CoordinateMode.ParentPercentageOffset, true);
            nameTextBox.SetLocationConfig(50, 30, CoordinateMode.ParentPercentageOffset, true);
            saveTypeRadioButtons.SetLocationConfig(10, 50, CoordinateMode.ParentPercentageOffset, false);
            saveButton.SetLocationConfig(30, 80, CoordinateMode.ParentPercentageOffset, true);
            cancelButton.SetLocationConfig(70, 80, CoordinateMode.ParentPercentageOffset, true);

            AddChild(_versionText);
            AddChild(saveButton);
            AddChild(cancelButton);
            AddChild(nameTextBox);
            AddChild(saveTypeRadioButtons);
        }

        private void _dialogOptionSelected(ChipSaveDialogOption option)
        {
            switch (option)
            {
                case ChipSaveDialogOption.SaveAsNewTemplate:
                    _versionText.Visible = false;
                    break;
                case ChipSaveDialogOption.SaveAsNewVersion:
                    _versionText.Visible = true;
                    break;
            }
        }

        public enum ChipSaveDialogOption
        {
            SaveAsNewTemplate,
            SaveAsNewVersion
        }

    }
}
