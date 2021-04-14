using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class SearchBarMenuItem : TextBoxMenuItem
    {
        public SearchBarMenuItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer,"")
        {
            TextItem.SetLocationConfig(4, 3, CoordinateMode.ParentPixelOffset, false);
            SpriteName = "SearchBar";
            Editable = true;
            MaxTextLength = 7;
        }
    }
}
