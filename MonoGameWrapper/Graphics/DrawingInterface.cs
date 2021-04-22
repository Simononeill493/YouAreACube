using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAmACube
{
    public class DrawingInterface
    {
        private PrimitivesHelper _primitivesHelper;
        public DrawingInterface(PrimitivesHelper primitivesHelper)
        {
            _primitivesHelper = primitivesHelper;
        }

        public void DrawBackground(string background) => _primitivesHelper.DrawStretchedToScreen(background);
        public void DrawText(string text, int x, int y, int scale, float layer,Color color,bool centered = false) => _primitivesHelper.DrawText(text, x, y, scale, layer, color, centered);
        public void DrawSprite(string sprite, int x, int y, int scale, float layer, Color colorMask,bool centered = false, bool flipHorizontal = false, bool flipVertical = false) => _primitivesHelper.DrawSprite(sprite, x, y, scale, layer, colorMask, centered,flipHorizontal,flipVertical);
        public void DrawRectangle(int x, int y, int width, int height, float layer, Color color, bool centered = false)=>_primitivesHelper.DrawRectangle(x,y,width,height,layer,color,centered);


        public void DrawTile(Tile tile, Point drawPos, CameraConfiguration cameraConfig)
        {
            DrawBlock(tile.Ground, drawPos, DrawLayers.GroundLayer, cameraConfig);

            if (tile.HasSurface)
            {
                DrawBlock(tile.Surface, drawPos, DrawLayers.SurfaceLayer, cameraConfig);
            }
            if (tile.HasEphemeral)
            {
                DrawBlock(tile.Ephemeral, drawPos, DrawLayers.EphemeralLayer, cameraConfig);
            }
        }

        public void DrawBlock(Block block, Point drawPos, float layer, CameraConfiguration cameraConfig)
        {
            drawPos += CameraUtils.GetMovementOffsets(block, cameraConfig.TileSizeActual);

            _primitivesHelper.DrawSprite(block.Sprite, drawPos.X, drawPos.Y, cameraConfig.Scale, layer, colorMask: Color.White,false,false,false);
        }


        public void DrawTileDebugOverlay(Tile tile, Point drawPos, CameraConfiguration cameraConfig)
        {
            _primitivesHelper.DrawText(tile.AbsoluteLocation.ToString(), drawPos.X, drawPos.Y, 2, DrawLayers.GameTileDebugLayer, Color.Black, false);
            if (tile.HasSurface)
            {
                DrawBlockDebugOverlay(tile.Surface, drawPos, DrawLayers.SurfaceLayer, cameraConfig);
            }
            if (tile.HasEphemeral)
            {
                DrawBlockDebugOverlay(tile.Ephemeral, drawPos, DrawLayers.EphemeralLayer, cameraConfig);
            }
        }

        public void DrawBlockDebugOverlay(Block block, Point drawPos, float layer, CameraConfiguration cameraConfig)
        {
            if(block.Active)
            {
                drawPos += CameraUtils.GetMovementOffsets(block, cameraConfig.TileSizeActual);

                var energyPercentage = block.EnergyRemainingPercentage;
                var barCurrentLength = energyPercentage * cameraConfig.TileSizeActual;
                _primitivesHelper.DrawRectangle(drawPos.X, drawPos.Y, cameraConfig.TileSizeActual, 8, DrawLayers.BlockInfoLayer_Back, Color.Black, false);
                _primitivesHelper.DrawRectangle(drawPos.X, drawPos.Y, (int)barCurrentLength, 8,DrawLayers.BlockInfoLayer_Front, Color.Cyan, false);
            }
        }

        public void DrawSectorGridOverlay(Point sector,int gridLineThickness,CameraConfiguration _config)
        {
            var sectorOrigin = sector * Config.SectorSize * _config.TileSizeActual;
            var sectorOriginOffset = sectorOrigin - _config.ActualOffset;
            var sectorSquareSize = _config.TileSizeActual * Config.SectorSize;

            DrawRectangle(sectorOriginOffset.X, sectorOriginOffset.Y, gridLineThickness, sectorSquareSize, DrawLayers.GameSectorOverlayLayer, Color.Red);
            DrawRectangle(sectorOriginOffset.X, sectorOriginOffset.Y, sectorSquareSize, gridLineThickness, DrawLayers.GameSectorOverlayLayer, Color.Red);
            DrawRectangle(sectorOriginOffset.X + sectorSquareSize, sectorOriginOffset.Y, gridLineThickness, sectorSquareSize, DrawLayers.GameSectorOverlayLayer, Color.Red);
            DrawRectangle(sectorOriginOffset.X, sectorOriginOffset.Y + sectorSquareSize, sectorSquareSize, gridLineThickness, DrawLayers.GameSectorOverlayLayer, Color.Red);
        }


        public void DrawVoid(Point drawPos, CameraConfiguration cameraConfig)
        {
            _primitivesHelper.DrawSprite("Black", drawPos.X, drawPos.Y, cameraConfig.Scale, DrawLayers.GroundLayer, colorMask: Color.White, false, false, false);
        }

        public void DrawHUD(Kernel kernel)
        {
            DrawHUD(kernel, MonoGameWindow.CurrentSize.X / 16, MonoGameWindow.CurrentSize.Y / 16);
        }
        public void DrawHUD(Kernel kernel, int x, int y)
        {
            var host = kernel.Host;
            _primitivesHelper.DrawRectangle(x - 2, y - 2, 204, 29, DrawLayers.HUDLayer, Color.Black,false);
            _primitivesHelper.DrawRectangle(x, y, (int)(200.0 * (host.EnergyRemainingPercentage)), 25, DrawLayers.HUDLayer, Color.Blue,false);
        }
    }
}
