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
            var world = new World(new Random().Next(),Config.DefaultSectorSize);
            var centre = GetTestSector(world.Random,new IntPoint(0, 0),world.SectorSize);
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
        public static Sector GetTestSector(Random r, IntPoint coords, int sectorSize)
        {
            var sector = _getInitializedSector(coords, sectorSize);
            _setBasicGround(sector);

            _testWorldGeneration(r, sector);

            return sector;
        }

        private static void _setBasicGround(Sector sector)
        {
            foreach (var tile in sector.TileGrid)
            {
                var ground = Templates.GenerateGround("grassPatch", 0);
                ground.EnterLocation(tile);
            }
        }


        private static void _testWorldGeneration(Random r, Sector sector)
        {
            var grid = new WorldGenGrid(sector.Size,r);

            //var shootEnemy = Templates.Database["ShootEnemy"][0];
            //grid.AddRandom(BlockMode.Surface, shootEnemy, 64);


            var rock = Templates.Database["Rock1"][0];
            var rock2 = Templates.Database["Rock2"][0];

            grid.AddRandom(BlockMode.Surface, rock, 16);
            grid.AddToSide(BlockMode.Surface, rock, 1, 2);
            grid.AddToSide(BlockMode.Surface, rock, 0.3, 2);
            grid.AddToSide(BlockMode.Surface, rock, 0.2, 2);
            grid.AddToSide(BlockMode.Surface, rock, 0.05, 2);

            //grid.AddRandom(BlockMode.Surface, rock2, 5);
            //grid.AddToSide(BlockMode.Surface, rock2, 0.1, 8);

            //grid.AddRandom(r, BlockMode.Surface, "ShootEnemy", 0, 4);
            //grid.AddRandom(r, BlockMode.Surface, "Rock2", 0, 128);

            grid.OverlayOnSector(sector);
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


        private static Sector _getInitializedSector(IntPoint sectorOrigin,int sectorSize)
        {
            var (tiles,tilesFlattened) = _getInitializedGrid(sectorOrigin, sectorSize);
            AttachmentUtils.AttachTileGridToSelf(tiles, sectorSize);

            var sector = new Sector(new IntPoint(sectorSize,sectorSize),sectorOrigin, tiles, tilesFlattened);
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