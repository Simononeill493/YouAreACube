using Microsoft.Xna.Framework;
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
            var world = new World(seed);
            var centre = GetTestSector(new Point(0, 0));
            world.AddSector(centre);

            //AttachmentUtils.AddOuterSectors(world);
            //AttachmentUtils.AddOuterSectors(world);

            //world.AddSector(GetTestSector(new Point(1, 0)));
            /*world.AddSector(GetTestSector(new Point(2, 0)));
            world.AddSector(GetTestSector(new Point(2, 1)));
            world.AddSector(GetTestSector(new Point(2, 2)));
            world.AddSector(GetTestSector(new Point(1, 2)));
            world.AddSector(GetTestSector(new Point(0, 2)));
            world.AddSector(GetTestSector(new Point(-1, 2)));
            world.AddSector(GetTestSector(new Point(-2, 2)));
            world.AddSector(GetTestSector(new Point(-2, 1)));
            world.AddSector(GetTestSector(new Point(-2, 0)));
            world.AddSector(GetTestSector(new Point(-1, 0)));*/

            //AttachmentUtils.AddOuterSectors(world);

            return world;
        }



        public static Sector GetTestSector(Point coords)
        {
            var sector = _getInitializedSector(coords);
            _setBasicGround(sector);

            return sector;
        }
        private static void _setBasicGround(Sector sector)
        {
            foreach (var tile in sector.TileGrid)
            {
                var ground = Templates.GenerateGround("grassPatch");
                tile.AddGround(ground);
            }
        }

        public static void AddPlayer(World world,SurfaceBlock player)
        {
            var centre = world.GetSector(new Point(0,0));
            var tile = centre.TileGrid[0, 0];

            tile.AddSurface(player);
            centre.AddToSector(player);
        }
        public static void AddEntities(World world)
        {
            var centre = world.GetSector(new Point(0, 0));

            _addRandom(world.Random, centre, BlockType.Surface, "BasicEnemy", 16);
            _addRandom(world.Random, centre, BlockType.Surface, "Spinner", 16);
            //_addRandom(world.Random, world.Centre, BlockType.Ephemeral,"Bullet", 16);
        }

        private static void _addRandom(Random r, Sector sector,BlockType blockType, string blockname,int number)
        {
            var emptyTiles = sector.Tiles.Where(t => t.Surface == null).ToList();
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

                var tile = emptyTiles[tileNum];
                tile.AddBlock(block);
                sector.AddToSector(block);

                emptyTiles.RemoveAt(tileNum);
                emptySize--;
            }
        }
        private static Sector _getInitializedSector(Point sectorOrigin)
        {
            var (tiles,tilesFlattened) = _getInitializedGrid(sectorOrigin, Config.SectorSize);
            AttachmentUtils.AttachTileGridToSelf(tiles,Config.SectorSize);

            var sector = new Sector(sectorOrigin, tiles, tilesFlattened);
            return sector;
        }
        private static (Tile[,] tiles, List<Tile> tilesFlattened) _getInitializedGrid(Point sectorOrigin,int size)
        {
            int xOrigin = size * sectorOrigin.X;
            int yOrigin = size * sectorOrigin.Y;

            var tiles = new Tile[size, size];
            var tilesFlattened = new List<Tile>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var xOffs = i + xOrigin;
                    var yOffs = j + yOrigin;

                    var tile = new Tile(new Point(i,j),new Point(xOffs, yOffs), sectorOrigin);
                    tiles[i, j] = tile;

                    tilesFlattened.Add(tile);
                }
            }

            return (tiles,tilesFlattened);
        }


    }
}