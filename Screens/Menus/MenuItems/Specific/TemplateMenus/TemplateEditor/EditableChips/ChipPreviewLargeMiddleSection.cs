using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipPreviewLargeMiddleSection : SpriteMenuItem
    {
        public ChipPreviewLargeMiddleSection(IHasDrawLayer parent,string inputType) : base(parent, "BlueChipFullMiddle") 
        {
            var textItem = new TextMenuItem(this);
            textItem.Text = inputType;
            textItem.MultiplyScale(0.5f);
            textItem.Color = Microsoft.Xna.Framework.Color.White;
            textItem.SetLocationConfig(4, 40, CoordinateMode.ParentPercentageOffset, false);

            AddChild(textItem);

            /*var dropdown = new DropdownMenuItem<int>(this);
            dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentageOffset, true);
            dropdown.SetItems(new List<int>() { 1, 2, 3, 4, 5 });

            AddChild(dropdown);*/
        }
    }
}
