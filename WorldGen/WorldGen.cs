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
            var centre = GetTestSector(world.Random,new IntPoint(0, 0),world.SectorSize,world.WorldKernel);
            //var centre = _getEmptySector(new IntPoint(0, 0), world.SectorSize);

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
        public static Sector GetTestSector(Random r, IntPoint coords, int sectorSize, Kernel worldKernel)
        {
            var sector = _getEmptySector(coords, sectorSize);

            _testWorldGeneration(r, sector,worldKernel);

            return sector;
        }

        private static void _testWorldGeneration(Random r, Sector sector,Kernel worldKernel)
        {
            var grid = new WorldGenGrid(worldKernel,sector.Size,r);

            var purpleCrystals = CrystalDatabase.CrystalTemplates["Purple"];
            grid.AddRandom(CubeMode.Surface, purpleCrystals, 5);

            var plants = Templates.Database["Plants1"][0];
            grid.AddRandom(CubeMode.Ground, plants, r.Next(8, 48));
            grid.AddToSide(CubeMode.Ground, plants, 0.1, 15);

            var rock = Templates.Database["Rock1"][0];
            grid.AddRandom(CubeMode.Surface, rock, r.Next(8,48));
            grid.AddToSide(CubeMode.Surface, rock, 0.1, 15);

            var rock2 = Templates.Database["Rock2"][0];
            grid.AddRandom(CubeMode.Surface, rock2, r.Next(4, 24));
            grid.AddToSide(CubeMode.Surface, rock2, 0.1, r.Next(8, 12));

            grid.AddRandom(CubeMode.Surface, Templates.Database["ShootEnemy"][0], r.Next(4, 6));
            grid.AddRandom(CubeMode.Surface, Templates.Database["ApproachEnemy"][0], r.Next(4, 6));
            grid.AddRandom(CubeMode.Surface, Templates.Database["FleeEnemy"][0], r.Next(4, 6));
            grid.AddRandom(CubeMode.Surface, Templates.Database["MouseFollower"][0], r.Next(4, 6));
            //grid.AddRandom(BlockMode.Surface, Templates.Database["Spinner"][0], r.Next(4, 16));

            /*grid.AddRandom(BlockMode.Surface, rock, 16);
            grid.AddToSide(BlockMode.Surface, rock, 1, 2);
            grid.AddToSide(BlockMode.Surface, rock, 0.3, 2);
            grid.AddToSide(BlockMode.Surface, rock, 0.2, 2);
            grid.AddToSide(BlockMode.Surface, rock, 0.05, 2);*/

            //grid.AddRandom(BlockMode.Surface, rock2, 5);
            //grid.AddToSide(BlockMode.Surface, rock2, 0.1, 8);

            //grid.AddRandom(r, BlockMode.Surface, "ShootEnemy", 0, 4);
            //grid.AddRandom(r, BlockMode.Surface, "Rock2", 0, 128);

            grid.OverlayOnSector(sector);
            FillEnergyBarsForSector(sector);
        }

        public static void FillEnergyBarsForSector(Sector sector)
        {
            foreach(var active in sector.ActiveBlocks)
            {
                active.AddEnergy(active.EnergyCap);
            }
        }


        public static void AddPlayer(World world,SurfaceCube player)
        {
            var centre = world.GetSector(new IntPoint(0,0));
            var tile = centre.TileGrid[0, 0];

            if(tile.HasSurface)
            {
                centre.RemoveBlockFromSector(tile.Surface);
            }

            centre.AddBlockToSector(player);
            player.EnterLocation(tile);
            player.AddEnergy(player.EnergyCap);

            world.FocusOn(player);
        }


        private static Sector _getEmptySector(IntPoint sectorOrigin,int sectorSize)
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