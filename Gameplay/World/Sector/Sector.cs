﻿using Microsoft.Xna.Framework;
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
        public Tile[,] TileGrid;
        public List<Tile> Tiles;
        public IEnumerable<Block> DoomedBlocks => _destructibleBlocks.Where(b => b.ShouldBeDestroyed());

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

        public ActionsList GetBlockActions(UserInput input,TickManager tickCounter)
        {
            var actions = new ActionsList();
            var toUpdate = tickCounter.GetUpdatingBlocks(this,ActiveBlocks);

            foreach(var block in toUpdate)
            {
                block.Update(input,actions);
            }

            return actions;
        }

        public void Update(ActionsList actions) => _updateManager.Update(actions);

        public Tile GetTile(IntPoint point)
        {
            var tile = TileGrid[point.X, point.Y];
            return tile;
        }

        public void AddNonMovingBlockToSector(Block block)
        {
            if (block.BlockType != BlockMode.Ground)
            {
                _destructibleBlocks.Add(block);
            }
            if (block.Active)
            {
                ActiveBlocks.Add(block);
            }
        }
        public void AddMovingBlockToSector(Block block)
        {
            AddNonMovingBlockToSector(block);
            _updateManager.AddToMoving(block);
        }

        public void RemoveFromSectorLists(Block block)
        {
            if (block.BlockType != BlockMode.Ground)
            {
                var destructibleRemoved =_destructibleBlocks.Remove(block);
                if(!destructibleRemoved)
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

        public List<(Block, IntPoint)> PopSectorEmmigrants()
        {
            var (movedOut,createdOut) = _updateManager.GetSectorEmmigrants();
            _updateManager.ClearSectorEmmigrants();

            foreach(var emmigrant in movedOut)
            {
                RemoveFromSectorLists(emmigrant.Item1);
            }

            movedOut.AddRange(createdOut);
            return movedOut;
        }
    }
}