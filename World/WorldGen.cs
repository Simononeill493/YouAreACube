using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class WorldGen
    {
        public static World GenerateFreshWorld()
        {
            var centre = _getEmptySector();
            var world = new World(centre);

            return world;
        }

        private static Sector _getEmptySector()
        {
            var tiles = _getBlankGrid(Config.SectorSize);
            _setGridAdjacents(tiles,Config.SectorSize);

            var sector = new Sector(tiles);
            return sector;
        }
        private static Tile[,] _getBlankGrid(int size)
        {
            var tiles = new Tile[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    tiles[i, j] = new Tile();
                }
            }

            return tiles;
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
