using Microsoft.Xna.Framework.Input;
using System;

namespace IAmACube
{
    public class CameraConfiguration
    {
        public bool DebugMode = true;

        public int Scale = 1;
        public int TileSizePixels;

        public IntPoint GridPosition;
        public IntPoint PartialGridOffset;
        public IntPoint PixelOffset;

        public IntPoint VisibleGrid = IntPoint.Zero;
        public IntPoint MouseHoverPosition = IntPoint.Zero;

        public void UpdateScaling()
        {
            if (Scale < 1) { Console.WriteLine("Warning: Camera scale is set to less than 1 (" + Scale + ")."); }

            TileSizePixels = Config.TileSizePixels * Scale;
            VisibleGrid = MonoGameWindow.CurrentSize / TileSizePixels;

            PixelOffset = (GridPosition * TileSizePixels) + PartialGridOffset;
        }
        public void RollOverGridOffsets()
        {
            if (PartialGridOffset.X > TileSizePixels)
            {
                GridPosition.X += (PartialGridOffset.X / TileSizePixels);
                PartialGridOffset.X %= TileSizePixels;
            }
            if (PartialGridOffset.Y > TileSizePixels)
            {
                GridPosition.Y += (PartialGridOffset.Y / TileSizePixels);
                PartialGridOffset.Y %= TileSizePixels;
            }
            if (PartialGridOffset.X < 0)
            {
                GridPosition.X -= ((-PartialGridOffset.X / TileSizePixels) + 1);
                PartialGridOffset.X = TileSizePixels - ((-PartialGridOffset.X % TileSizePixels));
            }
            if (PartialGridOffset.Y < 0)
            {
                GridPosition.Y -= ((-PartialGridOffset.Y / TileSizePixels) + 1);
                PartialGridOffset.Y = TileSizePixels - ((-PartialGridOffset.Y % TileSizePixels));
            }
        }


        public void ChangeScale(int offset)
        {
            var newScale = offset + Scale;
            if (newScale < 1)
            {
                return;
            }

            Scale = newScale;
        }
        public void SnapToBlock(Block block, IntPoint offset)
        {
            GridPosition = (block.Location.AbsoluteLocation - (VisibleGrid / 2));
            PartialGridOffset = offset + CameraUtils.GetMovementOffsets(block, TileSizePixels);

            UpdateScaling();
        }

        public IntPoint GetCameraCentre() => (VisibleGrid / 2 * TileSizePixels);
        public IntPoint GetPosOnScreen(Block block) => CameraUtils.GetBlockOffsetFromOrigin(block, TileSizePixels) - PixelOffset;
        public IntPoint GetMouseHover(UserInput input)
        {
            var mouseDivided = (input.MousePos + PixelOffset) / (float)TileSizePixels;
            return mouseDivided.Floor;
        }



        public void HandleUserInput(UserInput input)
        {
            PartialGridOffset += KeyUtils.GetRightKeypadDirection(input) * 15;
            ChangeScale(input.ScrollDirection);

            if (input.IsKeyJustPressed(Keys.F3))
            {
                DebugMode = !DebugMode;
            }
        }
        public void AssignMouseHover(UserInput input, World world)
        {
            var mousePos = GetMouseHover(input);
            if (world.HasTile(mousePos))
            {
                input.MouseHoverTile = world.GetTile(mousePos);
            }

            MouseHoverPosition = input.MouseHoverTile.AbsoluteLocation;
        }
    }
}
