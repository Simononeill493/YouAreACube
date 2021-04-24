using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class CameraConfiguration
    {
        public int Scale = 1;

        public Point GridPosition;
        public Point PartialGridOffset;
        public Point ActualOffset;

        public int TileSizeActual;

        public Point VisibleGrid = Point.Zero;

        public Point MouseHoverPosition = Point.Zero;

        public void UpdateScaling()
        {
            if (Scale < 1) {Console.WriteLine("Warning: Camera scale is set to less than 1 (" + Scale + ")."); }

            TileSizeActual = Config.TileSizePixels * Scale;
            VisibleGrid = MonoGameWindow.CurrentSize / TileSizeActual;

            ActualOffset = (GridPosition * TileSizeActual) + PartialGridOffset;
        }
        public void UpdateGridOffsets()
        {
            if (PartialGridOffset.X > TileSizeActual)
            {
                GridPosition.X += (PartialGridOffset.X / TileSizeActual);
                PartialGridOffset.X = PartialGridOffset.X % TileSizeActual;
            }
            if (PartialGridOffset.Y > TileSizeActual)
            {
                GridPosition.Y += (PartialGridOffset.Y / TileSizeActual);
                PartialGridOffset.Y = PartialGridOffset.Y % TileSizeActual;
            }
            if (PartialGridOffset.X < 0)
            {
                GridPosition.X -= ((-PartialGridOffset.X / TileSizeActual) + 1);
                PartialGridOffset.X = TileSizeActual - ((-PartialGridOffset.X % TileSizeActual));
            }
            if (PartialGridOffset.Y < 0)
            {
                GridPosition.Y -= ((-PartialGridOffset.Y / TileSizeActual) + 1);
                PartialGridOffset.Y = TileSizeActual - ((-PartialGridOffset.Y % TileSizeActual));
            }
        }

        public Point GetPosOnScreen(Block block)
        {
            return CameraUtils.GetBlockOffsetFromOrigin(block, TileSizeActual) - ActualOffset;
        }

        public (int x,int y) GetCameraCentre()
        {
            var xMidPoint = ((VisibleGrid.X / 2 * TileSizeActual));
            var yMidPoint = ((VisibleGrid.Y / 2 * TileSizeActual));

            return (xMidPoint, yMidPoint);
        }

        public void SnapToBlock(Block block,Point offset)
        {
            GridPosition.X = block.Location.AbsoluteLocation.X - (VisibleGrid.X / 2);
            GridPosition.Y = block.Location.AbsoluteLocation.Y - (VisibleGrid.Y/ 2);

            (PartialGridOffset) = CameraUtils.GetMovementOffsets(block,TileSizeActual);

            PartialGridOffset += offset;
        }
    }
}
