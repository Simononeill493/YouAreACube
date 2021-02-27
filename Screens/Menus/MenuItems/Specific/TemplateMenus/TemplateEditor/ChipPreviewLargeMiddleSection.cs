using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipPreviewLargeMiddleSection : SpriteMenuItem
    {
        public ChipPreviewLargeMiddleSection(IHasDrawLayer parent,string text) : base(parent, "BlueChipFullMiddle") 
        {
            var dropdown = new DropdownMenuItem<int>(this);
            dropdown.SetLocationConfig(70, 50, CoordinateMode.ParentPercentageOffset, true);
            dropdown.SetItems(new List<int>() { 1, 2, 3, 4, 5 });

            AddChild(dropdown);

            var textItem = new TextMenuItem(this);
            textItem.Text = text;
            textItem.HalfScaled = true;
            textItem.Color = Microsoft.Xna.Framework.Color.White;
            textItem.SetLocationConfig(10, 50, CoordinateMode.ParentPercentageOffset, true);

            AddChild(textItem);
        }

    }
}
