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

        public static Rectangle GetSpriteDimensions(string spriteName,int x,int y,int scale)
        {
            var sprite = SpriteManager.GetSprite(spriteName);
            return new Rectangle(x, y, sprite.Width * scale, sprite.Height * scale);
        }
    }
}
