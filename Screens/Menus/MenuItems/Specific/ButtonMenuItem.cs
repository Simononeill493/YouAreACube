using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ButtonMenuItem : TextBoxMenuItem
    {
        public ButtonMenuItem(IHasDrawLayer parentDrawLayer, string initialString) : base(parentDrawLayer,initialString)
        {
            box.SpriteName = "EmptyButtonRectangle";
        }
    }
}
