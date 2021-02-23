using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace IAmACube
{
    class ChipPreviewSmall : TextBoxMenuItem
    {
        public ChipPreviewSmall(IHasDrawLayer parent) : base(parent, "BlueChipSmall")
        {
            box.SpriteName = "BlueChipSmall";
            Editable = false;
            text.Color = Color.White;
            text.HalfScaled = true;
        }
    }
}
