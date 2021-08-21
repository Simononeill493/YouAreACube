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

        private MenuOrientation _orientation;
        private int _buttonDistance;
        private string _buttonSprite;

        public TabArrayMenuItem(IHasDrawLayer parent,MenuOrientation orientation,int buttonDistance,string buttonSprite = null) : base(parent, BuiltInMenuSprites.BlankPixel)
        {
            if(buttonSprite==null) { buttonSprite = BuiltInMenuSprites.BasicTabButton; }
            _tabButtons = new List<TabButtonMenuItem>();
            _orientation = orientation;
            _buttonDistance = buttonDistance;
            _buttonSprite = buttonSprite;

            Tabs = new List<MenuItem>();
        }

        public void AddTabButton(string name,MenuItem tab)
        {
            if(Tabs.Contains(tab))
            {
                throw new Exception("Tried to add the same tab twice to a tabarray");
            }

            var tabButton = _makeNewButton(name, tab);

            var tabSize = tabButton.GetBaseSize();
            var locationConfig = IntPoint.Zero;
            if(_orientation == MenuOrientation.Horizontal)
            {
                locationConfig.X = (tabSize.X + _buttonDistance) * (_countOfTabsInLine);
            }
            else if(_orientation == MenuOrientation.Vertical)
            {
                locationConfig.Y = (tabSize.Y + _buttonDistance) * (_countOfTabsInLine);
            }

            tabButton.SetLocationConfig(locationConfig, CoordinateMode.ParentPixelOffset, false);
            _countOfTabsInLine++;
        }

        public void AddTabButtonWithManualLocation(string name, MenuItem tab, int x, int y, CoordinateMode coordinateMode, bool centered)
        {
            var tabButton = _makeNewButton(name, tab);
            tabButton.SetLocationConfig(x,y,coordinateMode,centered);
        }


        private TabButtonMenuItem _makeNewButton(string name, MenuItem tab)
        {
            var tabButton = new TabButtonMenuItem(this, tab, name,_buttonSprite);
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
