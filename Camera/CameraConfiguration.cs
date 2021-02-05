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

        public int XGridPosition;
        public int YGridPosition;

        public int XPartialGridOffset;
        public int YPartialGridOffset;

        public int XOffsetTrue => (TileSizeScaled * XGridPosition) + XPartialGridOffset;
        public int YOffsetTrue => (TileSizeScaled * YGridPosition) + YPartialGridOffset;

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
        }
        public void RollOverPartialOffsets()
        {
            if (XPartialGridOffset > TileSizeScaled)
            {
                XGridPosition += (XPartialGridOffset / TileSizeScaled);
                XPartialGridOffset = XPartialGridOffset % TileSizeScaled;
            }
            if (YPartialGridOffset > TileSizeScaled)
            {
                YGridPosition += (YPartialGridOffset / TileSizeScaled);
                YPartialGridOffset = YPartialGridOffset % TileSizeScaled;
            }
            if (XPartialGridOffset < 0)
            {
                XGridPosition -= ((-XPartialGridOffset / TileSizeScaled) + 1);
                XPartialGridOffset = TileSizeScaled - ((-XPartialGridOffset % TileSizeScaled));
            }
            if (YPartialGridOffset < 0)
            {
                YGridPosition -= ((-YPartialGridOffset / TileSizeScaled) + 1);
                YPartialGridOffset = TileSizeScaled - ((-YPartialGridOffset % TileSizeScaled));
            }
        }

        public (int x, int y) GetPosOnScreen(Block block)
        {
            var (XPos, YPos) = CameraUtils.GetBlockOffsetFromOrigin(block,TileSizeScaled);
            return (XPos - XOffsetTrue, YPos - YOffsetTrue);
        }

        public (int x,int y) GetCameraCentre()
        {
            var xMidPoint = ((VisibleGridWidth / 2 * TileSizeScaled));
            var yMidPoint = ((VisibleGridHeight / 2 * TileSizeScaled));

            return (xMidPoint, yMidPoint);
        }

        public void ClampToBlock(Block block,int xOffset = 0, int yOffset = 0)
        {
            XGridPosition = block.Location.LocationInSector.X - (VisibleGridWidth / 2);
            YGridPosition = block.Location.LocationInSector.Y - (VisibleGridHeight / 2);

            (XPartialGridOffset, YPartialGridOffset) = CameraUtils.GetMovementOffsets(block,TileSizeScaled);

            XPartialGridOffset += xOffset;
            YPartialGridOffset += yOffset;
        }
    }
}
