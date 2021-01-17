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
        public List<Tile> TilesFlattened;

        public Sector(Tile[,] tiles, List<Tile> tilesFlattened)
        {
            Tiles = tiles;
            TilesFlattened = tilesFlattened;
        }

        public (Tile tile,bool hasTile) TryGetTile(int x,int y)
        {
            if(x<Config.SectorSize & y < Config.SectorSize & x>-1 & y>-1)
            {
                var tile = Tiles[x, y];
                return (tile,true);
            }

            return (null,false);
        }

    }
}
