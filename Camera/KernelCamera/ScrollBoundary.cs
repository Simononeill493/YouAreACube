using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public struct ScrollBoundary
    {
        public IntPoint BoundaryNumTiles;
        public IntPoint BoundarySize;

        public int Top;
        public int Left;
        public int Bottom;
        public int Right;

        public void Update(CameraConfiguration config)
        {
            var tileSize = config.TileSizePixels;

            //BoundaryNumTiles = (config.VisibleGrid / 2);
            BoundaryNumTiles = config.VisibleGrid/3;
            BoundarySize = BoundaryNumTiles * tileSize;

            Top = BoundarySize.Y - tileSize;
            Left = BoundarySize.X - tileSize;

            Bottom = (MonoGameWindow.CurrentSize.Y - BoundarySize.Y);
            Right = (MonoGameWindow.CurrentSize.X - BoundarySize.X);
        }

        public IntPoint GetBoundaryPushBack(IntPoint curPos)
        {
            var clampChange = IntPoint.Zero;

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

        public bool WithinBoundary(IntPoint kernelScreenPosition) => kernelScreenPosition.InBoundsInclusive(Left, Top, Right, Bottom);
    }
}
