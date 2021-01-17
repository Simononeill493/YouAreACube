using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class Camera
    {
        public int Scale = 1;
        public int XGridPos;
        public int YGridPos;

        public void DrawWorld(DrawingInterface drawingInterface,Sector sector)
        {
            DrawWorldGround(drawingInterface, sector);
        }

        public void DrawWorldGround(DrawingInterface drawingInterface, Sector sector)
        {
            int tileSizeScaled = Config.TileSizeActual * Scale;

            int tilesToDrawWidth = (MonoGameWindow.CurrentWidth / tileSizeScaled)+1;
            int tilesToDrawHeight = (MonoGameWindow.CurrentHeight / tileSizeScaled)+1;

            for (int i = 0; i < tilesToDrawWidth; i++)
            {
                for (int j = 0; j < tilesToDrawHeight; j++)
                {
                    int iOffs = i + XGridPos;
                    int jOffs = j + YGridPos;

                    Tile tile = null;
                    GroundBlock ground = null;

                    if(!sector.TryGetTile(iOffs, jOffs, out tile))
                    {
                        ground = Templates.VoidBlock;
                    }
                    else
                    {
                        ground = sector.Tiles[iOffs, jOffs].Ground;
                    }

                    drawingInterface.DrawSprite(ground.Background, i * tileSizeScaled, j * tileSizeScaled,Scale);
                }
            }
        }

        public void KeyInput(KeyboardState keyboardState, List<Keys> keysUp)
        {
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                YGridPos--;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                YGridPos++;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                XGridPos--;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
               XGridPos++;
            }
            if (keysUp.Contains(Keys.P))
            {
                Scale++;
            }
            if (keysUp.Contains(Keys.O))
            {
                if (Scale > 1) { Scale--; }
            }

        }

    }
}
