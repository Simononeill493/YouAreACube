using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipSaveDialog: DialogBoxMenuItem
    {
        public ChipSaveDialog(IHasDrawLayer parentDrawLayer, MenuItem container) : base(parentDrawLayer, container, "EmptyMenuRectangleMedium")
        {
            var saveAsNewVersionButton = new ButtonMenuItem(this, "Save New Version") { SpriteName = "EmptyButtonRectangleMedium" };
            var saveNewTemplateButton = new ButtonMenuItem(this, "Save New Template") { SpriteName = "EmptyButtonRectangleMedium" };
            var cancelButton = new ButtonMenuItem(this, "Cancel");
            var nameTextBox = new TextBoxMenuItem(this, "") { Editable = true };

            //saveAsNewVersionButton.TextItem.MultiplyScale(0.5f);
            //saveNewTemplateButton.TextItem.MultiplyScale(0.5f);
            //cancelButton.TextItem.MultiplyScale(0.5f);

            cancelButton.OnMouseReleased += (i) => Close();

            saveAsNewVersionButton.SetLocationConfig(50, 40, CoordinateMode.ParentPercentageOffset, true);
            saveNewTemplateButton.SetLocationConfig(50, 60, CoordinateMode.ParentPercentageOffset, true);
            cancelButton.SetLocationConfig(50, 80, CoordinateMode.ParentPercentageOffset, true);
            nameTextBox.SetLocationConfig(50, 20, CoordinateMode.ParentPercentageOffset, true);

            AddChild(saveAsNewVersionButton);
            AddChild(saveNewTemplateButton);
            AddChild(cancelButton);
            AddChild(nameTextBox);
        }
    }
}
