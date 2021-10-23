using Microsoft.Xna.Framework.Input;
using System;

namespace IAmACube
{
    public class CameraConfiguration
    {
        public bool DebugMode = false;

        public int Scale = 1;
        public int TileSizePixels;

        public IntPoint GridPosition;
        public IntPoint PartialGridOffset;
        public IntPoint PixelOffset;

        public IntPoint VisibleGrid = IntPoint.Zero;
        public IntPoint MouseHoverPosition = IntPoint.Zero;

        public int MinViewRange = 8;
        public int MaxViewRange = 20;

        public void Update(UserInput input)
        {
            MouseHoverPosition = input.MouseHoverTile.AbsoluteLocation;

            _handleUserInput(input);
            UpdateScaling();
        }

        public Tile GetMouseHoverTile(IntPoint mousePosAbsolute, World world)
        {
            var mouseTilePos = GetMouseHover(mousePosAbsolute);
            if (world.HasTile(mouseTilePos))
            {
                return world.GetTile(mouseTilePos);
            }

            return Tile.Dummy;
        }

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
            if(offset==0) { return; }

            if (!DebugMode & ((offset > 0 & VisibleGrid.Min < MinViewRange) | (offset < 0 & VisibleGrid.Max > MaxViewRange)))
            {
                return;
            }

            var newScale = offset + Scale;
            if (newScale < 1)
            {
                return;
            }

            Scale = newScale;
        }
        public void SnapToBlock(Cube block, IntPoint offset)
        {
            GridPosition = (block.Location.AbsoluteLocation - (VisibleGrid / 2));
            PartialGridOffset = offset + CameraUtils.GetMovementOffsets(block, TileSizePixels);

            UpdateScaling();
        }

        public IntPoint GetCameraCentre() => (VisibleGrid / 2 * TileSizePixels);
        public IntPoint GetPosOnScreen(Cube block) => CameraUtils.GetBlockOffsetFromOrigin(block, TileSizePixels) - PixelOffset;
        public IntPoint GetMouseHover(IntPoint mousePosAbsolute)
        {
            var mouseDivided = (mousePosAbsolute + PixelOffset) / (float)TileSizePixels;
            return mouseDivided.Floor;
        }

        private void _handleUserInput(UserInput input)
        {
            PartialGridOffset += KeyUtils.GetRightKeypadDirection(input) * 15;
            ChangeScale(input.ScrollDirection);

            if (input.IsKeyJustPressed(Keys.F3))
            {
                DebugMode = !DebugMode;
            }
        }


    }
}
