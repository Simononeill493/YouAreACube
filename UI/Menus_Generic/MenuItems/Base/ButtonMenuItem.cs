using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ButtonMenuItem : TextBoxMenuItem
    {
        public ButtonMenuItem(IHasDrawLayer parentDrawLayer, string buttontext) : base(parentDrawLayer, buttontext)
        {
            SpriteName = BuiltInMenuSprites.SmallRectangularButton;
        }
    }
}
