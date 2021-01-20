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
        public List<Tile> _tilesFlattened;

        private List<Block> _activeBlocks;

        public Sector(Tile[,] tiles, List<Tile> tilesFlattened)
        {
            Tiles = tiles;

            _tilesFlattened = tilesFlattened;
            _activeBlocks = new List<Block>();
        }

        public void Update(UserInput input,TickCounter tickCounter)
        {
            var toUpdate = _getUpdatingBlocks(tickCounter);
            foreach(var block in toUpdate)
            {
                block.Update(input);
            }
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

        public void AddBlockToSector(Block block,Tile tile)
        {
            if(tile.Contents!=null)
            {
                Console.WriteLine("Warning: adding a block to a sector at a tile that has already been filled.");
            }

            if(block.Active)
            {
                _activeBlocks.Add(block);
            }

            tile.Contents = block;
            block.Location = tile;
            //todo check EVERY time a block is moved that this is set 
        }
        public void AddGroundToSector(GroundBlock block, Tile tile)
        {
            if (tile.Contents != null)
            {
                Console.WriteLine("Warning: adding ground to a sector at a tile that has already been filled.");
            }

            tile.Ground= block;

        }
    
        private IEnumerable<Block> _getUpdatingBlocks(TickCounter tickCounter)
        {
            return _activeBlocks.Where(b => tickCounter.TotalTicks % b.Speed == 0);
        }
    }

}
