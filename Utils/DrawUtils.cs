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
    }
}
