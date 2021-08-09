using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TabArrayMenuItem : SpriteMenuItem
    {
        public List<MenuItem> Tabs;
        private List<TabButtonMenuItem> _tabButtons;
        private TabButtonMenuItem _activeTab;
        private int _countOfTabsInLine;

        public TabArrayMenuItem(IHasDrawLayer parent) : base(parent, "BlankPixel")
        {
            _tabButtons = new List<TabButtonMenuItem>();
            Tabs = new List<MenuItem>();
        }

        public void AddTabButton(string name,MenuItem tab)
        {
            var tabButton = _makeNewButton(name, tab);

            var tabSize = tabButton.GetBaseSize();
            var x = (tabSize.X+10) * (_countOfTabsInLine);
            tabButton.SetLocationConfig(x, 0, CoordinateMode.ParentPixelOffset, false);

            _countOfTabsInLine++;
        }

        public void AddTabButtonWithManualLocation(string name, MenuItem tab, int x, int y, CoordinateMode coordinateMode, bool centered)
        {
            var tabButton = _makeNewButton(name, tab);
            tabButton.SetLocationConfig(x,y,coordinateMode,centered);
        }


        private TabButtonMenuItem _makeNewButton(string name, MenuItem tab)
        {
            var tabButton = new TabButtonMenuItem(this, tab, name);
            tabButton.OnMouseReleased += (i) => SwitchToTab(tabButton);
            tab.Visible = false;
            tab.Enabled = false;

            AddChild(tabButton);
            _tabButtons.Add(tabButton);
            Tabs.Add(tab);

            return tabButton;
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
