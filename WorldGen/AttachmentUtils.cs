using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class AttachmentUtils
    {
        public static void AddOuterSectors(World world)
        {
            var curSectors = world.SectorGrid.Values.ToList();

            foreach (var sector in curSectors)
            {
                foreach (var cardinal in DirectionUtils.Cardinals)
                {
                    if (!sector.HasNeighbour(cardinal))
                    {
                        var coord = sector.AbsoluteLocation.GetAdjacentCoords(cardinal);
                        var newSector = WorldGen.GetTestSector(world.Random,coord,world.SectorSize);
                        world.AddSector(newSector);
                    }
                }
            }
        }

        public static void AttachToWorld(World world, Sector sector)
        {
            foreach (var coord in DirectionUtils.GetAdjacentCoords(sector.AbsoluteLocation))
            {
                if (world.HasSector(coord.Item2))
                {
                    var toAttach = world.GetSector(coord.Item2);

                    sector.Adjacent[coord.Item1] = toAttach;
                    toAttach.Adjacent[DirectionUtils.Reverse(coord.Item1)] = sector;

                    _attachTiles(world, sector);
                }
            }
        }

        private static void _attachTiles(World world, Sector sector)
        {
            foreach (var edge in _getEdges(sector))
            {
                foreach (var cardinal in DirectionUtils.Cardinals)
                {
                    if (!edge.HasNeighbour(cardinal))
                    {
                        var offs = edge.AbsoluteLocation.GetAdjacentCoords(cardinal);
                        if (world.HasTile(offs))
                        {
                            var tileToAttach = world.GetTile(offs);
                            edge.Adjacent[cardinal] = tileToAttach;
                            tileToAttach.Adjacent[DirectionUtils.Reverse(cardinal)] = edge;
                        }
                    }
                }
            }
        }

        public static void AttachTileGridToSelf(Tile[,] tiles, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var tile = tiles[i, j];
                    _attachIfValid(tiles, tile, size, i, j - 1, CardinalDirection.North);
                    _attachIfValid(tiles, tile, size, i + 1, j - 1, CardinalDirection.NorthEast);
                    _attachIfValid(tiles, tile, size, i + 1, j, CardinalDirection.East);
                    _attachIfValid(tiles, tile, size, i + 1, j + 1, CardinalDirection.SouthEast);
                    _attachIfValid(tiles, tile, size, i, j + 1, CardinalDirection.South);
                    _attachIfValid(tiles, tile, size, i - 1, j + 1, CardinalDirection.SouthWest);
                    _attachIfValid(tiles, tile, size, i - 1, j, CardinalDirection.West);
                    _attachIfValid(tiles, tile, size, i - 1, j - 1, CardinalDirection.NorthWest);
                }
            }

        }
        private static void _attachIfValid(Tile[,] tiles, Tile tile, int size, int x, int y, CardinalDirection direction)
        {
            if (x > -1 & y > -1 & x < size & y < size)
            {
                tile.Adjacent[direction] = tiles[x, y];
            }
        }

        private static IEnumerable<Tile> _getEdges(Sector sector) => sector.Tiles.Where(t => t.IsEdge);
    }
}
