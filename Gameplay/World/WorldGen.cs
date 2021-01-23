using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class WorldGen
    {
        public static World GenerateFreshWorld(Random seed)
        {
            var centre = _getInitializedSector();
            var world = new World(centre);

            _setBasicGround(world);
            _addRandom(seed,world.Centre,"BasicEnemy",50);
            _addRandom(seed,world.Centre, "ScaredEnemy", 50);

            return world;
        }

        private static void _addRandom(Random seed, Sector sector, string blockname,int number)
        {
            var emptyTiles = sector._tilesFlattened.Where(t => t.Contents == null).ToList();
            var emptySize = emptyTiles.Count();

            for (int i = 0; i < number; i++)
            {
                if(emptySize==0)
                {
                    Console.WriteLine("Warning: Tried to add " + blockname + " but sector is full!");
                    return;
                }

                var block = Templates.GenerateSurfaceFromTemplate(blockname);
                var tileNum = seed.Next(0, emptySize - 1);

                sector.AddSurfaceToSector(block, emptyTiles[tileNum]);
                emptyTiles.RemoveAt(tileNum);
                emptySize--;
            }
        }
        private static void _setBasicGround(World world)
        {
            foreach(var sector in world.Sectors)
            {
                foreach(var tile in sector.Tiles)
                {
                    var ground = Templates.GenerateGroundFromTemplate("grassPatch");
                    sector.AddGroundToSector(ground, tile);
                }
            }
        }

        private static Sector _getInitializedSector()
        {
            var (tiles,tilesFlattaned) = _getInitializedGrid(Config.SectorSize);
            _setGridAdjacents(tiles,Config.SectorSize);

            var sector = new Sector(tiles, tilesFlattaned);
            return sector;
        }
        private static (Tile[,],List<Tile>) _getInitializedGrid(int size)
        {
            var tiles = new Tile[size, size];
            var tilesFlattened = new List<Tile>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var tile = new Tile(i,j);
                    tiles[i, j] = tile;
                    tilesFlattened.Add(tile);
                }
            }

            return (tiles,tilesFlattened);
        }

        private static void _setGridAdjacents(Tile[,] tiles,int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var tile = tiles[i, j];
                    _setAdjIfValid(tiles,tile, size, i, j - 1, Direction.Top);
                    _setAdjIfValid(tiles,tile, size, i + 1, j - 1, Direction.TopRight);
                    _setAdjIfValid(tiles,tile, size, i + 1, j, Direction.Right);
                    _setAdjIfValid(tiles,tile, size, i + 1, j + 1, Direction.BottomRight);
                    _setAdjIfValid(tiles,tile, size, i, j + 1, Direction.Bottom);
                    _setAdjIfValid(tiles,tile, size, i - 1, j + 1, Direction.BottomLeft);
                    _setAdjIfValid(tiles,tile, size, i - 1, j, Direction.Left);
                    _setAdjIfValid(tiles,tile, size, i - 1, j - 1, Direction.TopLeft);

                }
            }

        }
        private static void _setAdjIfValid(Tile[,] tiles,Tile tile, int size, int x, int y, Direction direction)
        {
            if (x > -1 & y > -1 & x < size & y < size)
            {
                tile.Adjacent[direction] = tiles[x, y];
            }
        }
    }
}
