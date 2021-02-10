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

        public int TileSizeScaled;
        public int VisibleGridWidth;
        public int VisibleGridHeight;

        public void SetScreenScaling()
        {
            if (Scale < 1)
            {
                Console.WriteLine("Warning: Camera scale is set to less than 1 (" + Scale + ").");
            }

            TileSizeScaled = Config.TileSizeActual * Scale;
            VisibleGridWidth = (MonoGameWindow.CurrentWidth / TileSizeScaled) + 1;
            VisibleGridHeight = (MonoGameWindow.CurrentHeight / TileSizeScaled) + 1;

            ActualOffset = (GridPosition * TileSizeScaled) + PartialGridOffset;
        }
        public void RollOverPartialOffsets()
        {
            if (PartialGridOffset.X > TileSizeScaled)
            {
                GridPosition.X += (PartialGridOffset.X / TileSizeScaled);
                PartialGridOffset.X = PartialGridOffset.X % TileSizeScaled;
            }
            if (PartialGridOffset.Y > TileSizeScaled)
            {
                GridPosition.Y += (PartialGridOffset.Y / TileSizeScaled);
                PartialGridOffset.Y = PartialGridOffset.Y % TileSizeScaled;
            }
            if (PartialGridOffset.X < 0)
            {
                GridPosition.X -= ((-PartialGridOffset.X / TileSizeScaled) + 1);
                PartialGridOffset.X = TileSizeScaled - ((-PartialGridOffset.X % TileSizeScaled));
            }
            if (PartialGridOffset.Y < 0)
            {
                GridPosition.Y -= ((-PartialGridOffset.Y / TileSizeScaled) + 1);
                PartialGridOffset.Y = TileSizeScaled - ((-PartialGridOffset.Y % TileSizeScaled));
            }
        }

        public Point GetPosOnScreen(Block block)
        {
            return CameraUtils.GetBlockOffsetFromOrigin(block, TileSizeScaled) - ActualOffset;
        }

        public (int x,int y) GetCameraCentre()
        {
            var xMidPoint = ((VisibleGridWidth / 2 * TileSizeScaled));
            var yMidPoint = ((VisibleGridHeight / 2 * TileSizeScaled));

            return (xMidPoint, yMidPoint);
        }

        public void SnapToBlock(Block block,Point offset)
        {
            GridPosition.X = block.Location.AbsoluteLocation.X - (VisibleGridWidth / 2);
            GridPosition.Y = block.Location.AbsoluteLocation.Y - (VisibleGridHeight / 2);

            (PartialGridOffset) = CameraUtils.GetMovementOffsets(block,TileSizeScaled);

            PartialGridOffset += offset;
        }
    }
}
