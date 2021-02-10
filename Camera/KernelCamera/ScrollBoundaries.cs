using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public struct ScrollBoundaries
    {
        public int BoundarySize;

        public int Top;
        public int Left;
        public int Bottom;
        public int Right;

        public void Update(int borderNumTiles,int tileSize)
        {
            BoundarySize = borderNumTiles * tileSize;

            Top = BoundarySize - tileSize;
            Left = BoundarySize - tileSize;

            Bottom = (MonoGameWindow.CurrentHeight - BoundarySize);
            Right = (MonoGameWindow.CurrentWidth - BoundarySize);
        }

        public bool WithinBoundary(Point curPos)
        {
            return curPos.InBoundsInclusive(Left, Top, Right, Bottom);
        }

        public Point GetBoundaryPushBack(Point curPos)
        {
            var clampChange = Point.Zero;

            if (curPos.X <= Left)
            {
                var difference = (Left - curPos.X);
                clampChange.X -= (difference);
            }
            if (curPos.X >= Right)
            {
                var difference = (curPos.X - Right);
                clampChange.X += (difference);
            }
            if (curPos.Y <= Top)
            {
                var difference = (Top - curPos.Y);
                clampChange.Y -= (difference);
            }
            if (curPos.Y >= Bottom)
            {
                var difference = (curPos.Y - Bottom);
                clampChange.Y += (difference);
            }

            return clampChange;
        }
    }
}
