using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class SearchBarMenuItem : TextBoxMenuItem
    {
        public SearchBarMenuItem(IHasDrawLayer parentDrawLayer, Func<string> getText, Action<string> setText) : base(parentDrawLayer,getText,setText)
        {
            _textItem.SetLocationConfig(4, 3, CoordinateMode.ParentPixel, false);
            SpriteName = MenuSprites.SearchBar;
            Editable = true;
            MaxTextLength = 7;
        }
    }
}
