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
        public readonly IntPoint Size;

        public List<Cube> ActiveBlocks;
        private List<Cube> _destructibleBlocks;

        private SectorUpdateManager _updateManager;

        public Sector(IntPoint size,IntPoint location,Tile[,] tileGrid, List<Tile> tiles) : base(location)
        {
            Size = size;
            TileGrid = tileGrid;
            Tiles = tiles;

            _updateManager = new SectorUpdateManager(this);

            ActiveBlocks = new List<Cube>();
            _destructibleBlocks = new List<Cube>();
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


        public void AddBlockToSector(Cube block)
        {
            _updateManager.AddBlockToUpdates(block);

            _destructibleBlocks.Add(block);

            if (block.Active)
            {
                ActiveBlocks.Add(block);
            }
        }
        public void RemoveBlockFromSector(Cube block)
        {
            _updateManager.RemoveBlockFromUpdates(block);

            var destructibleRemoved = _destructibleBlocks.Remove(block);
            if (!destructibleRemoved)
            {
                throw new Exception("Tried to remove a block, but it wasn't in the expected sector");
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


        public IEnumerable<Cube> GetDoomedBlocks() => _destructibleBlocks.Where(b => b.ToBeDeleted());
        public SectorEmmigrantsList PopSectorEmmigrants()
        {
            var emmigrants = _updateManager.PopSectorEmmigrants();
            emmigrants.Moved.ForEach(moved => RemoveBlockFromSector(moved.Block));

            return emmigrants;
        }
        public Tile GetTile(IntPoint point) => TileGrid[point.X, point.Y];
    }
}