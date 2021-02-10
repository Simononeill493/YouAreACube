using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class DrawManager
    {
        private DrawingInterface _drawingInterface;
        public void SetDrawingInterface(DrawingInterface drawingInterface)
        {
            _drawingInterface = drawingInterface;
        }

        public void DrawHUD(Kernel kernel)
        {
            _drawingInterface.DrawHUD(kernel, MonoGameWindow.CurrentWidth / 16, MonoGameWindow.CurrentHeight / 16);
        }
        public void DrawVoid(Point drawPos, CameraConfiguration cameraConfig)
        {
            _drawingInterface.DrawSprite("Black", drawPos.X, drawPos.Y, CameraDrawLayer.GroundLayer, cameraConfig.Scale);
        }
        public void DrawTile(Tile tile, Point drawPos,CameraConfiguration cameraConfig)
        {
            _drawTileSprite(tile.Ground, drawPos, CameraDrawLayer.GroundLayer, cameraConfig);

            if (tile.HasSurface)
            {
                _drawTileSprite(tile.Surface, drawPos, CameraDrawLayer.SurfaceLayer, cameraConfig);
            }
            if (tile.HasEphemeral)
            {
                _drawTileSprite(tile.Ephemeral, drawPos, CameraDrawLayer.EphemeralLayer, cameraConfig);
            }
        }
        private void _drawTileSprite(Block block, Point drawPos, float layer, CameraConfiguration cameraConfig)
        {
            var offsetDrawPos = drawPos + CameraUtils.GetMovementOffsets(block, cameraConfig.TileSizeScaled);

            _drawingInterface.DrawSprite(block.Sprite, offsetDrawPos.X, offsetDrawPos.Y, layer, cameraConfig.Scale);
        }
    }
}
