using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class TabButtonMenuItem : TextBoxMenuItem
    {
        public ScreenItem Tab;
        public TabButtonMenuItem(IHasDrawLayer parentDrawLayer, ScreenItem tab,string initialString,string buttonSprite) : base(parentDrawLayer, initialString)
        {
            Tab = tab;
            SpriteName = buttonSprite;
            _textItem.MultiplyScale(0.5f);
        }
    }
}
