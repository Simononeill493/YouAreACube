using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TabButtonMenuItem : TextBoxMenuItem
    {
        public ScreenItem Tab;
        public TabButtonMenuItem(IHasDrawLayer parentDrawLayer, ScreenItem tab, string initialString, string buttonSprite) : base(parentDrawLayer, initialString)
        {
            Tab = tab;
            SpriteName = buttonSprite;
            _textItem.MultiplyScale(0.5f);

            HighlightColor = Color.DarkGoldenrod;
        }

        public void Select()
        {
            Tab.ShowAndEnable();
            DefaultColor = Color.Cyan;
        }

        public void Deselect()
        {
            Tab.HideAndDisable();
            ResetColor();
        }
    }
}
