using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class DrawUtils
    {
        public static (int xOffset, int yOffset) GetCenteredCoords(int width, int height, int x, int y, int scale)
        {
            var xOffset = x - ((int)((width / 2.0) * scale));
            var yOffset = y - ((int)((height / 2.0) * scale));

            return (xOffset, yOffset);
        }

        public static (int x, int y) ScreenPercentageToCoords(int xPercentage, int yPercentage)
        {
            var x = (int)(MonoGameWindow.CurrentWidth * (xPercentage / 100.0));
            var y = (int)(MonoGameWindow.CurrentHeight * (yPercentage / 100.0));

            return (x, y);
        }

        public static Rectangle GetMenuItemRectangle(MenuItem item)
        //public static (int x1, int y1, int x2, int y2) GetMenuItemRectangle(MenuItem item)
        {
            var sprite = SpriteManager.GetSprite(item.SpriteName);

            var (x, y) = ScreenPercentageToCoords(item.XPercentage, item.YPercentage);
            var (x1, y1) = GetCenteredCoords(sprite.Width, sprite.Height, x, y, item.Scale);

            var x2 = x1 + sprite.Width * item.Scale;
            var y2 = y1 + sprite.Height * item.Scale;

            return new Rectangle(x1, y1, sprite.Width * item.Scale, sprite.Height * item.Scale);
            //return (x1, y1, x2, y2);
        }
    }
}
