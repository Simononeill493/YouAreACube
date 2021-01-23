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

        private List<Block> _activeBlocks;

        public Sector(Tile[,] tiles, List<Tile> tilesFlattened)
        {
            Tiles = tiles;
            TilesFlattened = tilesFlattened;

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

        public void AddSurfaceToSector(SurfaceBlock block,Tile tile)
        {
            if(tile.Contents!=null)
            {
                Console.WriteLine("Warning: adding a surface block to a sector at a tile that has already been filled.");
            }

            tile.Contents = block;
            _addBlockToSector(block, tile);

        }
        public void AddGroundToSector(GroundBlock block, Tile tile)
        {
            if (tile.Ground != null)
            {
                Console.WriteLine("Warning: adding ground to a sector at a tile that has already been filled.");
            }

            tile.Ground= block;
            _addBlockToSector(block, tile);
        }
        private void _addBlockToSector(Block block,Tile tile)
        {
            block.Location = tile;
            //todo ensure EVERY time a block is added that this is set 

            if (block.Active)
            {
                _activeBlocks.Add(block);
            }
        }

        private IEnumerable<Block> _getUpdatingBlocks(TickCounter tickCounter)
        {
            return _activeBlocks.Where(b => (tickCounter.TotalTicks+b.SpeedOffset) % b.Speed == 0);
        }
    }

}
