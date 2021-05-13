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
        private DrawingInterfacePrimitive _primitive;
        public DrawingInterface(DrawingInterfacePrimitive primitive)
        {
            _primitive = primitive;
        }

        public void DrawBackground(string background) => _primitive.DrawStretchedToScreen(background);
        public void DrawText(string text, int x, int y, int scale, float layer,Color color,bool centered = false) => _primitive.DrawText(text, x, y, scale, layer, color, centered);
        public void DrawSprite(string sprite, int x, int y, int scale, float layer, Color colorMask,bool centered = false, bool flipHorizontal = false, bool flipVertical = false) => _primitive.DrawSprite(sprite, x, y, scale, layer, colorMask, centered,flipHorizontal,flipVertical);
        public void DrawRectangle(IntPoint location, int width, int height, float layer, Color color, bool centered = false) => _primitive.DrawRectangle(location.X, location.Y, width, height, layer, color, centered);
        public void DrawRectangle(int x, int y, int width, int height, float layer, Color color, bool centered = false)=>_primitive.DrawRectangle(x,y,width,height,layer,color,centered);


        public void DrawTile(Tile tile, IntPoint drawPos, CameraConfiguration cameraConfig)
        {
            _primitive.DrawSprite(tile.Sprite, drawPos.X, drawPos.Y, cameraConfig.Scale, DrawLayers.TileLayer, Color.White, false, false, false);

            if (tile.HasGround)
            {
                DrawBlock(tile.Ground, drawPos, DrawLayers.GroundLayer, cameraConfig);
            }
            if (tile.HasSurface)
            {
                DrawBlock(tile.Surface, drawPos, DrawLayers.SurfaceLayer, cameraConfig);
            }
            if (tile.HasEphemeral)
            {
                DrawBlock(tile.Ephemeral, drawPos, DrawLayers.EphemeralLayer, cameraConfig);
            }

            if(cameraConfig.DebugMode)
            {
                DrawTileDebugOverlay(tile, drawPos, cameraConfig);
            }
        }
        public void DrawBlock(Block block, IntPoint drawPos, float layer, CameraConfiguration cameraConfig)
        {
            drawPos += CameraUtils.GetMovementOffsets(block, cameraConfig.TileSizePixels);
            var colorMask = new Color(block.ColorMask.Item1, block.ColorMask.Item2, block.ColorMask.Item3, block.ColorMask.Item4);

            _primitive.DrawSprite(block.Sprite, drawPos.X, drawPos.Y, cameraConfig.Scale, layer, colorMask, false,false,false);
        }
        public void DrawVoid(IntPoint drawPos, CameraConfiguration cameraConfig) => _primitive.DrawSprite("Black", drawPos.X, drawPos.Y, cameraConfig.Scale, DrawLayers.GroundLayer, Color.White, false, false, false);


        public void DrawTileDebugOverlay(Tile tile, IntPoint drawPos, CameraConfiguration cameraConfig)
        {
            //_primitivesHelper.DrawText(tile.AbsoluteLocation.ToString(), drawPos.X, drawPos.Y, 2, DrawLayers.GameTileDebugLayer, Color.Black, false);
            
            if (tile.HasSurface)
            {
                DrawBlockEnergyOverlay(tile.Surface, drawPos, cameraConfig);
                DrawBlockHealthOverlay(tile.Surface, drawPos, cameraConfig);
            }
            if (tile.HasEphemeral)
            {
                DrawBlockEnergyOverlay(tile.Ephemeral, drawPos, cameraConfig);
            }
        }
        public void DrawBlockEnergyOverlay(Block block, IntPoint drawPos, CameraConfiguration cameraConfig)
        {
            if(block.Active)
            {
                drawPos += CameraUtils.GetMovementOffsets(block, cameraConfig.TileSizePixels);

                var energyPercentage = block.EnergyRemainingPercentage;
                var barCurrentLength = energyPercentage * cameraConfig.TileSizePixels;
                _primitive.DrawRectangle(drawPos.X, drawPos.Y, cameraConfig.TileSizePixels, 8, DrawLayers.BlockInfoLayer_Back, Color.Black, false);
                _primitive.DrawRectangle(drawPos.X, drawPos.Y, (int)barCurrentLength, 8,DrawLayers.BlockInfoLayer_Front, Color.Cyan, false);
            }
        }
        public void DrawBlockHealthOverlay(SurfaceBlock block, IntPoint drawPos, CameraConfiguration cameraConfig)
        {
            drawPos += CameraUtils.GetMovementOffsets(block, cameraConfig.TileSizePixels);

            var healthPercentage = block.HealthRemainingPercentage;

            if(healthPercentage<1)
            {
                var barCurrentLength = healthPercentage * cameraConfig.TileSizePixels;
                _primitive.DrawRectangle(drawPos.X, drawPos.Y + 8, cameraConfig.TileSizePixels, 8, DrawLayers.BlockInfoLayer_Back, Color.Black, false);
                _primitive.DrawRectangle(drawPos.X, drawPos.Y + 8, (int)barCurrentLength, 8, DrawLayers.BlockInfoLayer_Front, Color.Red, false);
            }
        }
        public void DrawSectorGridOverlay(IntPoint sector,int sectorSize,int gridLineThickness,CameraConfiguration _config)
        {
            var sectorOrigin = sector * sectorSize * _config.TileSizePixels;
            var sectorOriginOffset = sectorOrigin - _config.PixelOffset;
            var sectorSquareSize = _config.TileSizePixels * sectorSize;

            DrawRectangle(sectorOriginOffset.X, sectorOriginOffset.Y, gridLineThickness, sectorSquareSize, DrawLayers.GameSectorOverlayLayer, Color.Red);
            DrawRectangle(sectorOriginOffset.X, sectorOriginOffset.Y, sectorSquareSize, gridLineThickness, DrawLayers.GameSectorOverlayLayer, Color.Red);
            DrawRectangle(sectorOriginOffset.X + sectorSquareSize, sectorOriginOffset.Y, gridLineThickness, sectorSquareSize, DrawLayers.GameSectorOverlayLayer, Color.Red);
            DrawRectangle(sectorOriginOffset.X, sectorOriginOffset.Y + sectorSquareSize, sectorSquareSize, gridLineThickness, DrawLayers.GameSectorOverlayLayer, Color.Red);
        }
    }
}
