using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TabArrayMenuItem : SpriteMenuItem
    {
        private List<TabButtonMenuItem> _tabButtons;
        public List<MenuItem> Tabs;
        private TabButtonMenuItem _activeTab;

        public TabArrayMenuItem(IHasDrawLayer parent) : base(parent, "BlankPixel")
        {
            _tabButtons = new List<TabButtonMenuItem>();
            Tabs = new List<MenuItem>();
        }

        public void AddTab(string name,MenuItem tab)
        {
            var tabButton = new TabButtonMenuItem(this, tab,name);
            //tabButton.TextItem.Color = Microsoft.Xna.Framework.Color.White;
            var tabSize = tabButton.GetBaseSize();

            var x = (tabSize.X+10) * (Tabs.Count);

            tabButton.SetLocationConfig(x, -tabSize.Y, CoordinateMode.ParentPixelOffset, false);
            tabButton.OnMouseReleased += (i) => SwitchToTab(tabButton);
            AddChild(tabButton);

            _tabButtons.Add(tabButton);
            Tabs.Add(tab);
        }

        public void SwitchToTab(TabButtonMenuItem tabButton)
        {
            if(_activeTab!=null)
            {
                _activeTab.Tab.Visible = false;
                _activeTab.Tab.Enabled = false;
            }

            tabButton.Tab.Visible = true;
            tabButton.Tab.Enabled = true;
            _activeTab = tabButton;
        }

        public void SwitchToFirstTab() => SwitchToTab(_tabButtons.First());
    }
}
