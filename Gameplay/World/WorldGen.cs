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
            var centre = GetTestSector(new IntPoint(0, 0));
            world.AddSector(centre);

            AttachmentUtils.AddOuterSectors(world);
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



        public static Sector GetTestSector(IntPoint coords)
        {
            var sector = _getInitializedSector(coords);
            _setBasicGround(sector);

            return sector;
        }
        private static void _setBasicGround(Sector sector)
        {
            foreach (var tile in sector.TileGrid)
            {
                var ground = Templates.GenerateGround("grassPatch",0);
                ground.EnterLocation(tile);
                //tile.AddGround(ground);
            }
        }

        public static void AddPlayer(World world,SurfaceBlock player)
        {
            var centre = world.GetSector(new IntPoint(0,0));
            var tile = centre.TileGrid[0, 0];

            player.EnterLocation(tile);
            world.FocusOn(player);
            //tile.AddSurface(player);
            centre.AddNonMovingBlockToSector(player);
        }
        public static void AddEntities(World world)
        {
            var centre = world.GetSector(new IntPoint(0, 0));

            //_addRandom(world.Random, centre, BlockMode.Surface, "MouseFollower", 0, 32);
            //_addRandom(world.Random, centre, BlockMode.Surface, "ScaredEnemy",0, 16);
            //_addRandom(world.Random, centre, BlockMode.Surface, "Spinner",0, 16);
            //_addRandom(world.Random, world.Centre, BlockType.Ephemeral,"Bullet", 16);
        }

        private static void _addRandom(Random r, Sector sector,BlockMode blockType, string blockname,int version,int number)
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

                var block = Templates.Generate(blockname, version,blockType);
                var tileNum = r.Next(0, emptySize - 1);

                var tile = emptyTiles[tileNum];
                block.EnterLocation(tile);
                //tile.AddBlock(block);
                sector.AddNonMovingBlockToSector(block);

                emptyTiles.RemoveAt(tileNum);
                emptySize--;
            }
        }
        private static Sector _getInitializedSector(IntPoint sectorOrigin)
        {
            var (tiles,tilesFlattened) = _getInitializedGrid(sectorOrigin, Config.SectorSize);
            AttachmentUtils.AttachTileGridToSelf(tiles,Config.SectorSize);

            var sector = new Sector(sectorOrigin, tiles, tilesFlattened);
            return sector;
        }
        private static (Tile[,] tiles, List<Tile> tilesFlattened) _getInitializedGrid(IntPoint sectorOrigin,int size)
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

                    var tile = new Tile(new IntPoint(i,j),new IntPoint(xOffs, yOffs), sectorOrigin);
                    tiles[i, j] = tile;

                    tilesFlattened.Add(tile);
                }
            }

            return (tiles,tilesFlattened);
        }


    }
}