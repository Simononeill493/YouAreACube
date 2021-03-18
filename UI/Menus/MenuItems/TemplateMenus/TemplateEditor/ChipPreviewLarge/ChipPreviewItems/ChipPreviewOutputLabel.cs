using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipPreviewOutputLabel : TextBoxMenuItem
    {
        public ChipPreviewOutputLabel(IHasDrawLayer parent,ChipData chip) : base(parent, chip.Name + "_out")
        {
            var textItem = new TextMenuItem(this);
            textItem.Text = chip.Output;
            textItem.MultiplyScale(0.5f);
            textItem.Color = Microsoft.Xna.Framework.Color.Black;
            textItem.SetLocationConfig(50, 15, CoordinateMode.ParentPercentageOffset, true);

            AddChild(textItem);

            Editable = true;
        }
    }
}
