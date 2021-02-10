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

        public MenuScreen(Action<ScreenType> switchScreen) : base(switchScreen) { }

        public override void Draw(DrawingInterface drawingInterface)
        {
            if(Background!=null)
            {
                drawingInterface.DrawBackground(Background);
            }

            foreach (var item in MenuItems)
            {
                drawingInterface.DrawMenuItem(item);
            }
        }

        public override void Update(UserInput input)
        {
            foreach (var item in MenuItems)
            {
                item.Update(input);
            }
        }
    }
}
