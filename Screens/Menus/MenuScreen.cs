using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    abstract class MenuScreen : Screen
    {
        public string Background;
        public List<MenuItem> MenuItems = new List<MenuItem>();
        public List<(MenuItem,int, int, PositioningMode)> MenuItemLocations = new List<(MenuItem, int, int, PositioningMode)>();

        public MenuScreen(Action<ScreenType> switchScreen) : base(switchScreen) { }

        public override void Draw(DrawingInterface drawingInterface)
        {
            if(Background!=null)
            {
                drawingInterface.DrawBackground(Background);
            }

            foreach (var item in MenuItems)
            {
                item.Draw(drawingInterface);
            }
        }

        public override void Update(UserInput input)
        {
            _updateMenuPositions();

            foreach (var item in MenuItems)
            {
                item.Update(input);
            }
        }

        protected void _setMenuPosition(MenuItem item, int x, int y, PositioningMode mode)
        {
            MenuItemLocations.Add((item, x, y, mode));
            item.SetLocation(x, y, mode);
        }

        private void _updateMenuPositions()
        {
            foreach(var item in MenuItemLocations)
            {
                if(item.Item4 == PositioningMode.Relative)
                {
                    item.Item1.SetLocation(item.Item2, item.Item3,item.Item4);
                }
            }
        }
    }
}
