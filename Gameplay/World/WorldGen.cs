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
            var world = new World(seed,Config.DefaultSectorSize);
            var centre = GetTestSector(new IntPoint(0, 0),world.SectorSize);
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



        public static Sector GetTestSector(IntPoint coords,int sectorSize)
        {
            var sector = _getInitializedSector(coords,sectorSize);
            _setBasicGround(sector);

            return sector;
        }
        private static void _setBasicGround(Sector sector)
        {
            foreach (var tile in sector.TileGrid)
            {
                var ground = Templates.GenerateGround("grassPatch",0);
                ground.EnterLocation(tile);
            }
        }

        public static void AddPlayer(World world,SurfaceBlock player)
        {
            var centre = world.GetSector(new IntPoint(0,0));
            var tile = centre.TileGrid[0, 0];

            if(tile.HasSurface)
            {
                centre.RemoveBlockFromSector(tile.Surface);
            }

            centre.AddBlockToSector(player);
            player.EnterLocation(tile);

            world.FocusOn(player);
        }
        public static void AddEntities(World world)
        {
            var centre = world.GetSector(new IntPoint(0, 0));

            //_addRandom(world.Random, centre, BlockMode.Surface, "MouseFollower", 0, 32);
            _addRandom(world.Random, centre, BlockMode.Surface, "ScaredEnemy",0, 32);
            //_addRandom(world.Random, centre, BlockMode.Surface, "Spinner",0, 16);
            //_addRandom(world.Random, world.Centre, BlockType.Ephemeral,"Bullet", 16);
        }

        private static void _addRandom(Random r, Sector sector,BlockMode blockType, string blockname,int version,int number)
        {
            var emptyTiles = sector.Tiles.Where(t => t.Surface == null).ToList();
            emptyTiles = RandomUtils.GetShuffledList(emptyTiles,r);

            var emptySize = emptyTiles.Count();
            if(emptySize < number)
            {
                Console.WriteLine("Worldgen warning: tried to add " + number + " blocks to a sector, but there are only " + emptySize + " free tiles.");
                number = emptySize;
            }

            for (int i = 0; i < number; i++)
            {
                var block = Templates.Generate(blockname, version,blockType);
                var tile = emptyTiles[i];

                block.EnterLocation(tile);
                sector.AddBlockToSector(block);
            }
        }
        private static Sector _getInitializedSector(IntPoint sectorOrigin,int sectorSize)
        {
            var (tiles,tilesFlattened) = _getInitializedGrid(sectorOrigin, sectorSize);
            AttachmentUtils.AttachTileGridToSelf(tiles, sectorSize);

            var sector = new Sector(sectorOrigin, tiles, tilesFlattened);
            return sector;
        }
        private static (Tile[,] tiles, List<Tile> tilesFlattened) _getInitializedGrid(IntPoint sectorOrigin,int sectorSize)
        {
            int xOrigin = sectorSize * sectorOrigin.X;
            int yOrigin = sectorSize * sectorOrigin.Y;

            var tiles = new Tile[sectorSize, sectorSize];
            var tilesFlattened = new List<Tile>();

            for (int i = 0; i < sectorSize; i++)
            {
                for (int j = 0; j < sectorSize; j++)
                {
                    var xOffs = i + xOrigin;
                    var yOffs = j + yOrigin;

                    var tile = new Tile(new IntPoint(i, j), new IntPoint(xOffs, yOffs), sectorOrigin, sectorSize);
                    tiles[i, j] = tile;

                    tilesFlattened.Add(tile);
                }
            }

            return (tiles,tilesFlattened);
        }


    }
}