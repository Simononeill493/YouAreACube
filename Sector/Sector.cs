using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class Sector
    {
        public Tile[,] Tiles;

        public Sector()
        {
            _initalizeGrid();
            _setAdjacents();
        }

        public void _initalizeGrid()
        {
            int size = Config.SectorSize;
            Tiles = new Tile[size,size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Tiles[i,j] = new Tile();
                }
            }

        }
        public void _setAdjacents()
        {
            int size = Config.SectorSize;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var tile = Tiles[i,j];
                    _setAdjIfValid(tile, size, i, j-1, Direction.Top);
                    _setAdjIfValid(tile, size, i + 1, j-1, Direction.TopRight);
                    _setAdjIfValid(tile, size, i + 1, j, Direction.Right);
                    _setAdjIfValid(tile, size, i + 1, j+1, Direction.BottomRight);
                    _setAdjIfValid(tile, size, i, j+1, Direction.Bottom);
                    _setAdjIfValid(tile, size, i - 1, j+1, Direction.BottomLeft);
                    _setAdjIfValid(tile, size, i - 1, j, Direction.Left);
                    _setAdjIfValid(tile, size, i - 1, j-1, Direction.TopLeft);

                }
            }

        }
        public void _setAdjIfValid(Tile tile,int size,int x,int y,Direction direction)
        {
            if (x > -1 & y>-1 & x<size & y< size)
            {
                tile.Adjacent[direction] = Tiles[x,y];
            }
        }
    }
}
