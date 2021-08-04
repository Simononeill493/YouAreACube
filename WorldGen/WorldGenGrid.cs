using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class WorldGenGrid
    {
        public readonly IntPoint Size;
        public readonly WorldGenGridPoint[,] TilesGrid;
        public readonly Dictionary<IntPoint,WorldGenGridPoint> TilesDict;

        private Random r;

        public WorldGenGrid(Kernel worldKernel,IntPoint size,Random random)
        {
            Size = size;
            TilesGrid = new WorldGenGridPoint[size.X,size.Y];
            TilesDict = new Dictionary<IntPoint, WorldGenGridPoint>();
            for(int i=0;i<size.X;i++)
            {
                for(int j=0;j<size.Y;j++)
                {
                    var point = new WorldGenGridPoint(worldKernel);
                    TilesGrid[i, j] = point;
                    TilesDict[new IntPoint(i, j)] = point;
                }
            }

            r = random;   
        }

        public void OverlayOnSector(Sector s)
        {
            if(Size!=s.Size)
            {
                throw new Exception("Overlaid worldgen grid on sector of the wrong size");
            }

            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    var worldGenPoint = TilesGrid[i, j];
                    var actualTile = s.TileGrid[i, j];
                    worldGenPoint.OverlayOnTile(s, actualTile);
                }
            }
        }

        public void AddRandom(CubeMode blockType, CubeTemplate template, int number)
        {
            var emptyTiles = TilesDict.Values.Where(t => !t.Has(blockType)).ToList();
            emptyTiles = RandomUtils.GetShuffledList(emptyTiles, r);

            var emptySize = emptyTiles.Count();
            if (emptySize < number)
            {
                Console.WriteLine("Worldgen warning: tried to add " + number + " blocks to a sector, but there are only " + emptySize + " free tiles.");
                number = emptySize;
            }

            for (int i = 0; i < number; i++)
            {
                emptyTiles[i].Set(blockType, template);
            }
        }

        public void AddToSide(CubeMode blockMode, CubeTemplate template,double odds,int numTiles)
        {
            for(int i=0;i<numTiles;i++)
            {
                AddToSide(blockMode, template, odds);
            }
        }
        public void AddToSide(CubeMode blockMode, CubeTemplate template, double odds)
        {
            var tilesWithThisTemplate = TilesDict.Where(t => !t.Value.Surrounded[blockMode] & t.Value.Get(blockMode)==template).ToList();

            foreach(var point in tilesWithThisTemplate)
            {
                if(r.NextDouble() < odds)
                {
                    foreach (var side in point.Key.GetAdjacentPoints())
                    {
                        if (InBounds(side))
                        {
                            var sidePoint = TilesDict[side];
                            if (!sidePoint.Has(blockMode))
                            {
                                sidePoint.Set(blockMode, template);
                            }
                        }
                    }

                    point.Value.Surrounded[blockMode] = true;
                }
            }
        }





        public bool InBounds(IntPoint point)=>(point.X > -1 & point.Y > -1 & point.X < Size.X & point.Y < Size.Y);
    }

    public class WorldGenGridPoint
    {
        private Kernel _worldKernel;

        public string TileSprite;

        public CubeTemplate Surface;
        public CubeTemplate Ground;
        public CubeTemplate Ephemeral;

        public Dictionary<CubeMode, bool> Surrounded;
        public WorldGenGridPoint(Kernel worldKernel)
        {
            _worldKernel = worldKernel;
            TileSprite = "Grass";

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
            t.Sprite = TileSprite;

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