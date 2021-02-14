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
        private List<MenuItem> MenuItems = new List<MenuItem>();

        public MenuScreen(Action<ScreenType> switchScreen) : base(switchScreen) {}

        protected void AddMenuItem(MenuItem item)
        {
            MenuItems.Add(item);
            item.UpdateThisAndChildLocations();
        }

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
            foreach (var item in MenuItems)
            {
                item.Update(input);
            }
        }

        protected void _manuallyUpdateLocations()
        {
            foreach (var item in MenuItems)
            {
                item.UpdateLocation();
            }
        }
    }
}
