using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class Sector
    {
        public Tile[,] Tiles;

        public Sector(Tile[,] tiles)
        {
            Tiles = tiles;
        }

        public bool TryGetTile(int x,int y,out Tile tile)
        {
            tile = null;

            if(x<Config.SectorSize & y < Config.SectorSize & x>-1 & y>-1)
            {
                tile = Tiles[x, y];
                return true;
            }

            return false;
        }

    }
}
