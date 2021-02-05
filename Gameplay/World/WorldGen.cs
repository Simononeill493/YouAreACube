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
            var centre = GetTestSector(0,0);
            var world = new World(seed,centre);
            AttachmentUtils.AddOuterSectors(world);

            return world;
        }





        public static Sector GetTestSector(int xOffs, int yOffs)
        {
            var sector = _getInitializedSector(xOffs, yOffs);
            _setBasicGround(sector);

            return sector;
        }
        private static void _setBasicGround(Sector sector)
        {
            foreach (var tile in sector.Tiles)
            {
                var ground = Templates.GenerateGround("grassPatch");
                sector.AddGroundToSector(ground, tile);
            }
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
        private static Sector _getInitializedSector(int xOrigin,int yOrigin)
        {
            var (tiles,tilesFlattened) = _getInitializedGrid(Config.SectorSize,xOrigin * Config.SectorSize,yOrigin * Config.SectorSize);
            AttachmentUtils.AttachTileGridToSelf(tiles,Config.SectorSize);

            var sector = new Sector(xOrigin,yOrigin,tiles, tilesFlattened);
            return sector;
        }
        private static (Tile[,] tiles, List<Tile> tilesFlattened) _getInitializedGrid(int size, int xOrigin, int yOrigin)
        {
            var tiles = new Tile[size, size];
            var tilesFlattened = new List<Tile>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var xOffs = i + xOrigin;
                    var yOffs = j + yOrigin;

                    var tile = new Tile(i,j,xOffs, yOffs);
                    tiles[i, j] = tile;

                    tilesFlattened.Add(tile);
                }
            }

            return (tiles,tilesFlattened);
        }


    }
}