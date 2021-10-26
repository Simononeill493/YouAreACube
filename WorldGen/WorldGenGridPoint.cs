using System;
using System.Collections.Generic;

namespace IAmACube
{
    public class WorldGenGridPoint
    {
        private Kernel _worldKernel;

        public string TileSprite;
        public XnaColors TileSpriteMask = XnaColors.ClearColorMask;

        public CubeTemplate Surface;
        public CubeTemplate Ground;
        public CubeTemplate Ephemeral;

        public Dictionary<CubeMode, bool> Surrounded;
        public WorldGenGridPoint(Kernel worldKernel,string tileSprite)
        {
            _worldKernel = worldKernel;
            TileSprite = tileSprite;

            Surrounded = new Dictionary<CubeMode, bool>();
            Surrounded[CubeMode.Surface] = false;
            Surrounded[CubeMode.Ground] = false;
            Surrounded[CubeMode.Ephemeral] = false;
        }

        public bool Has(CubeMode blockMode)
        {
            switch (blockMode)
            {
                case CubeMode.Surface:
                    return Surface != null;
                case CubeMode.Ground:
                    return Ground != null;
                case CubeMode.Ephemeral:
                    return Ephemeral != null;
            }

            throw new Exception();
        }
        public CubeTemplate Get(CubeMode blockMode)
        {
            switch (blockMode)
            {
                case CubeMode.Surface:
                    return Surface;
                case CubeMode.Ground:
                    return Ground;
                case CubeMode.Ephemeral:
                    return Ephemeral;
            }

            throw new Exception();
        }
        public void Set(CubeMode blockMode,CubeTemplate template)
        {
            switch (blockMode)
            {
                case CubeMode.Surface:
                    Surface = template;
                    break;
                case CubeMode.Ground:
                    Ground = template;
                    break;
                case CubeMode.Ephemeral:
                    Ephemeral = template;
                    break;
            }
        }

        public void OverlayOnTile(Sector s,Tile t)
        {
            t.Sprite = new CubeSpriteDataSingle(TileSprite);
            t.Sprite.ColorMask = TileSpriteMask;

            if (Surface != null)
            {
                var block = Surface.GenerateSurface(_worldKernel);
                block.EnterLocation(t);
                s.AddBlockToSector(block);
            }

            if (Ground != null)
            {
                var block = Ground.GenerateGround(_worldKernel);
                block.EnterLocation(t);
                s.AddBlockToSector(block);
            }

            if (Ephemeral != null)
            {
                var block = Ephemeral.GenerateEphemeral(_worldKernel);
                block.EnterLocation(t);
                s.AddBlockToSector(block);
            }

        }
    }
}