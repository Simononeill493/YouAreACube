using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class MenuItem
    {
        public string SpriteName;
        public int XPercentage;
        public int YPercentage;
        public int Scale = 1;

        public bool Hovering = false;

        public string HighlightedSpriteName;
        public bool Highlightable = false;


        public bool Clickable = false;
        public bool ClickedOn = false;
        public Action ClickAction;
    }
}
