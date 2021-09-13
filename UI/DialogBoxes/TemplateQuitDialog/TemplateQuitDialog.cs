using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class TemplateQuitDialog: DialogBoxMenuItem
    {
        public TemplateQuitDialog(IHasDrawLayer parentDrawLayer, MenuItem container, Action<TemplateQuitButtonOption> onButtonPressed) : base(parentDrawLayer, container, BuiltInMenuSprites.MediumMenuRectangle)
        {
            _addStaticTextItem("Save before quitting?", 50, 20, CoordinateMode.ParentPercentageOffset, true);
            _addButton("Cancel", 50, 80, CoordinateMode.ParentPercentageOffset, true, (i) => Close());

            _addButton("Save and quit", 50, 40, CoordinateMode.ParentPercentageOffset, true, (i) =>
            {
                Close();
                onButtonPressed(TemplateQuitButtonOption.SaveAndQuit);
            });

            _addButton("Save appearance only", 50, 50, CoordinateMode.ParentPercentageOffset, true, (i) =>
            {
                Close();
                onButtonPressed(TemplateQuitButtonOption.SaveAppearanceOnly);
            });

            _addButton("Quit without saving", 50, 60, CoordinateMode.ParentPercentageOffset, true, (i) =>
            {
                Close();
                onButtonPressed(TemplateQuitButtonOption.QuitWithoutSaving);
            });
        }
    }
}
