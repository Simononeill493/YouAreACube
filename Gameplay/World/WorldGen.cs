using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class WorldGen
    {
        public static World GenerateEmptyWorld(int seed)
        {
            var centre = _getInitializedSector();
            var world = new World(seed,centre);

            _setBasicGround(world);

            return world;
        }

        public static void AddPlayer(World world,SurfaceBlock player)
        {
            var centre = world.Centre;
            var tile = centre.Tiles[0, 0];

            centre.AddSurfaceToSector(player, tile);
        }

        public static void AddEntities(World world)
        {
            _addRandom(world.Random, world.Centre, BlockType.Surface, "BasicEnemy", 16);
            _addRandom(world.Random, world.Centre, BlockType.Surface, "Spinner", 16);
            _addRandom(world.Random, world.Centre, BlockType.Ephemeral,"Bullet", 16);
        }

        private static void _addRandom(Random r, Sector sector,BlockType blockType, string blockname,int number)
        {
            var emptyTiles = sector.TilesFlattened.Where(t => t.Surface == null).ToList();
            var emptySize = emptyTiles.Count();

            for (int i = 0; i < number; i++)
            {
                if(emptySize==0)
                {
                    Console.WriteLine("Warning: Tried to add " + blockname + " but sector is full!");
                    return;
                }

                var block = Templates.Generate(blockname,blockType);
                var tileNum = r.Next(0, emptySize - 1);

                sector.AddBlockToSector(block, emptyTiles[tileNum]);
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
                    var ground = Templates.GenerateGround("grassPatch");
                    sector.AddGroundToSector(ground, tile);
                }
            }
        }

        private static Sector _getInitializedSector()
        {
            var (tiles,tilesFlattened) = _getInitializedGrid(Config.SectorSize);
            _setGridAdjacents(tiles,Config.SectorSize);

            var sector = new Sector(tiles, tilesFlattened);
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
                    _setAdjIfValid(tiles,tile, size, i, j - 1, CardinalDirection.North);
                    _setAdjIfValid(tiles,tile, size, i + 1, j - 1, CardinalDirection.NorthEast);
                    _setAdjIfValid(tiles,tile, size, i + 1, j, CardinalDirection.East);
                    _setAdjIfValid(tiles,tile, size, i + 1, j + 1, CardinalDirection.SouthEast);
                    _setAdjIfValid(tiles,tile, size, i, j + 1, CardinalDirection.South);
                    _setAdjIfValid(tiles,tile, size, i - 1, j + 1, CardinalDirection.SouthWest);
                    _setAdjIfValid(tiles,tile, size, i - 1, j, CardinalDirection.West);
                    _setAdjIfValid(tiles,tile, size, i - 1, j - 1, CardinalDirection.NorthWest);

                }
            }

        }
        private static void _setAdjIfValid(Tile[,] tiles,Tile tile, int size, int x, int y, CardinalDirection direction)
        {
            if (x > -1 & y > -1 & x < size & y < size)
            {
                tile.Adjacent[direction] = tiles[x, y];
            }
        }
    }
}
