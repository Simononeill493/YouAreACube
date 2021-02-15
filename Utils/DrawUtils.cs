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

        public static Point ScreenPercentageToCoords(Point percentages)
        {
            var x = (int)(MonoGameWindow.CurrentWidth * (percentages.X / 100.0));
            var y = (int)(MonoGameWindow.CurrentHeight * (percentages.Y / 100.0));

            return new Point(x, y);
        }

        public static Rectangle GetMenuItemRectangle(string spriteName,MenuItemDimensions dims,int scale)
        {
            var sprite = SpriteManager.GetSprite(spriteName);
            var location = dims.ActualLocation;

            if (dims.IsCentered)
            {
                var (x1, y1) = GetCenteredCoords(sprite.Width, sprite.Height, location.X, location.Y, scale);
                location = new Point(x1, y1);
            }

            return new Rectangle(location.X, location.Y, sprite.Width * scale, sprite.Height * scale);
        }
    }
}
