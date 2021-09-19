using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class TemplateQuitDialog: DialogBoxMenuItem
    {
        public TemplateQuitDialog(IHasDrawLayer parentDrawLayer, ScreenItem container, Action<TemplateQuitButtonOption> onButtonPressed) : base(parentDrawLayer, container, MenuSprites.MediumMenuRectangle)
        {
            _addStaticTextItem("Save before quitting?", 50, 20, CoordinateMode.ParentPercentage, true);
            _addButton("Cancel", 50, 80, CoordinateMode.ParentPercentage, true, (i) => Close());

            _addButton("Save and quit", 50, 40, CoordinateMode.ParentPercentage, true, (i) =>
            {
                Close();
                onButtonPressed(TemplateQuitButtonOption.SaveAndQuit);
            });

            _addButton("Save appearance only", 50, 50, CoordinateMode.ParentPercentage, true, (i) =>
            {
                Close();
                onButtonPressed(TemplateQuitButtonOption.SaveAppearanceOnly);
            });

            _addButton("Quit without saving", 50, 60, CoordinateMode.ParentPercentage, true, (i) =>
            {
                Close();
                onButtonPressed(TemplateQuitButtonOption.QuitWithoutSaving);
            });
        }
    }
}
