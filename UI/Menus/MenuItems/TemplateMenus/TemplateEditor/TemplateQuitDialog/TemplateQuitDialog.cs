using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class TemplateQuitDialog: DialogBoxMenuItem
    {
        public TemplateQuitDialog(IHasDrawLayer parentDrawLayer, MenuItem container, Action<TemplateQuitButtonOption> onButtonPressed) : base(parentDrawLayer, container, "EmptyMenuRectangleMedium")
        {
            var saveBeforeQuittingText = new TextMenuItem(this, "Save before quitting?");
            var saveAndQuitButton = new ButtonMenuItem(this, "Save and quit");
            var quitWithoutSavingButton = new ButtonMenuItem(this, "Quit without saving");
            var cancelButton = new ButtonMenuItem(this, "Cancel");

            saveAndQuitButton.OnMouseReleased += (i) => 
            {
                Close();
                onButtonPressed(TemplateQuitButtonOption.SaveAndQuit);
            };

            quitWithoutSavingButton.OnMouseReleased += (i) =>
            {
                Close();
                onButtonPressed(TemplateQuitButtonOption.QuitWithoutSaving);
            };

            cancelButton.OnMouseReleased += (i) => Close();

            saveBeforeQuittingText.SetLocationConfig(50, 20, CoordinateMode.ParentPercentageOffset, true);
            saveAndQuitButton.SetLocationConfig(50, 40, CoordinateMode.ParentPercentageOffset, true);
            quitWithoutSavingButton.SetLocationConfig(50, 60, CoordinateMode.ParentPercentageOffset, true);
            cancelButton.SetLocationConfig(50, 80, CoordinateMode.ParentPercentageOffset, true);
            
            AddChild(saveBeforeQuittingText);
            AddChild(saveAndQuitButton);
            AddChild(quitWithoutSavingButton);
            AddChild(cancelButton);
        }
    }
}
