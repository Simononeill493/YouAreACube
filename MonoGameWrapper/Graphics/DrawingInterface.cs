﻿using Microsoft.Xna.Framework;
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
        public void DrawText(string text, int xPercentage, int yPercentage, int scale, float layer) => _primitivesHelper.DrawText(text, xPercentage, yPercentage, scale, layer);
        
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
            var offsetDrawPos = drawPos + CameraUtils.GetMovementOffsets(block, cameraConfig.TileSizeScaled);

            _primitivesHelper.DrawSprite(block.Sprite, offsetDrawPos.X, offsetDrawPos.Y, cameraConfig.Scale, layer);
        }
        public void DrawVoid(Point drawPos, CameraConfiguration cameraConfig)
        {
            _primitivesHelper.DrawSprite("Black", drawPos.X, drawPos.Y, cameraConfig.Scale, DrawLayers.GroundLayer);
        }

        public void DrawHUD(Kernel kernel)
        {
            DrawHUD(kernel, MonoGameWindow.CurrentWidth / 16, MonoGameWindow.CurrentHeight / 16);
        }
        public void DrawHUD(Kernel kernel, int x, int y)
        {
            var host = kernel.Host;
            _primitivesHelper.DrawRectangle(x - 2, y - 2, 204, 29, DrawLayers.HUDLayer, Color.Black);
            _primitivesHelper.DrawRectangle(x, y, (int)(200.0 * (host.EnergyRemainingPercentage)), 25, DrawLayers.HUDLayer, Color.Blue);
        }

        public void DrawMenuItem(MenuItem item)
        {
            var (x, y) = DrawUtils.ScreenPercentageToCoords(item.XPercentage, item.YPercentage);

            if (item.Highlightable & item.Hovering)
            {
                _primitivesHelper.DrawSpriteCentered(item.HighlightedSpriteName, x, y, item.Scale, layer: DrawLayers.MenuItemLayer);
            }
            else
            {
                _primitivesHelper.DrawSpriteCentered(item.SpriteName, x, y, item.Scale, layer: DrawLayers.MenuItemLayer);
            }

            if (item.HasText)
            {
                _primitivesHelper.DrawText(item.Text, item.XPercentage, item.YPercentage, 2, layer: DrawLayers.MenuTextLayer);
            }
        }
    }
}
