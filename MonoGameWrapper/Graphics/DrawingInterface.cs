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
        public void DrawSprite(string sprite, int x, int y, int scale, float layer, Color colorMask,bool centered = false) => _primitivesHelper.DrawSprite(sprite, x, y, scale, layer, colorMask, centered);

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
            var offsetDrawPos = drawPos + CameraUtils.GetMovementOffsets(block, cameraConfig.TileSizeActual);

            _primitivesHelper.DrawSprite(block.Sprite, offsetDrawPos.X, offsetDrawPos.Y, cameraConfig.Scale, layer, colorMask: Color.White,centered: false);
        }
        public void DrawVoid(Point drawPos, CameraConfiguration cameraConfig)
        {
            _primitivesHelper.DrawSprite("Black", drawPos.X, drawPos.Y, cameraConfig.Scale, DrawLayers.GroundLayer, colorMask: Color.White,centered: false);
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
