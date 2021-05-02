using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class Sector : LocationWithNeighbors<Sector>
    {
        public readonly Tile[,] TileGrid;
        public readonly List<Tile> Tiles;

        public List<Block> ActiveBlocks;
        private List<Block> _destructibleBlocks;

        private SectorUpdateManager _updateManager;

        public Sector(IntPoint location,Tile[,] tileGrid, List<Tile> tiles) : base(location)
        {
            _updateManager = new SectorUpdateManager(this);

            TileGrid = tileGrid;
            Tiles = tiles;

            ActiveBlocks = new List<Block>();
            _destructibleBlocks = new List<Block>();
        }

        public SectorEmmigrantsList Tick(UserInput input, WorldTickManager tickManager)
        {
            var actions = _runChips(input, tickManager);
            _updateManager.Update(actions);

            return PopSectorEmmigrants();
        }

        public ActionsList _runChips(UserInput input, WorldTickManager tickManager)
        {
            var actions = new ActionsList();
            var updating = tickManager.GetUpdatingBlocks(this);

            foreach (var block in updating)
            {
                block.Update(input, actions);
            }

            return actions;
        }


        public void AddBlockToSector(Block block)
        {
            _updateManager.AddBlockToUpdates(block);

            if (block.BlockType != BlockMode.Ground)
            {
                _destructibleBlocks.Add(block);
            }
            if (block.Active)
            {
                ActiveBlocks.Add(block);
            }
        }
        public void RemoveBlockFromSector(Block block)
        {
            _updateManager.RemoveBlockFromUpdates(block);

            if (block.BlockType != BlockMode.Ground)
            {
                var destructibleRemoved = _destructibleBlocks.Remove(block);
                if (!destructibleRemoved)
                {
                    throw new Exception("Tried to remove a block, but it wasn't in the expected sector");
                }
            }
            if (block.Active)
            {
                var activeRemoved = ActiveBlocks.Remove(block);
                if (!activeRemoved)
                {
                    throw new Exception("Tried to remove a block, but it wasn't in the expected sector");
                }

            }

        }


        public IEnumerable<Block> GetDoomedBlocks() => _destructibleBlocks.Where(b => b.ToBeDeleted());
        public SectorEmmigrantsList PopSectorEmmigrants()
        {
            var emmigrants = _updateManager.PopSectorEmmigrants();
            emmigrants.Moved.ForEach(moved => RemoveBlockFromSector(moved.Block));

            return emmigrants;
        }
        public Tile GetTile(IntPoint point) => TileGrid[point.X, point.Y];
    }
}