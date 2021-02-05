﻿using Microsoft.Xna.Framework;
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
        public List<Tile> Edges;

        private List<Block> _activeBlocks;
        private List<Block> _destructibleBlocks;

        public Point Location;
        public Dictionary<CardinalDirection, Sector> Adjacent;

        public Sector(Point location,Tile[,] tiles, List<Tile> tilesFlattened)
        {
            Location = location;
            Tiles = tiles;
            TilesFlattened = tilesFlattened;

            _activeBlocks = new List<Block>();
            _destructibleBlocks = new List<Block>();
            Adjacent = new Dictionary<CardinalDirection, Sector>();

            Edges = TilesFlattened.Where(t => t.IsEdge).ToList();
        }

        public ActionsList UpdateBlocks(UserInput input,TickCounter tickCounter)
        {
            var actions = new ActionsList();
            var toUpdate = _getBlocksUpdatingOnThisTick(tickCounter);

            foreach(var block in toUpdate)
            {
                block.Update(input,actions);
            }

            return actions;
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

        public void AddBlockToSector(Block block, Tile tile)
        {
            switch (block.BlockType)
            {
                case BlockType.Surface:
                    AddSurfaceToSector((SurfaceBlock)block, tile);
                    break;
                case BlockType.Ground:
                    AddGroundToSector((GroundBlock)block, tile);
                    break;
                case BlockType.Ephemeral:
                    AddEphemeralToSector((EphemeralBlock)block, tile);
                    break;
            }
        }
        public void AddSurfaceToSector(SurfaceBlock block,Tile tile)
        {
            if(tile.HasSurface)
            {
                Console.WriteLine("Warning: adding a surface block to a sector at a tile that has already been filled.");
            }

            tile.Surface = block;
            _addBlockToSector(block, tile);
        }
        public void AddEphemeralToSector(EphemeralBlock block, Tile tile)
        {
            if (tile.HasEphemeral)
            {
                Console.WriteLine("Warning: adding a surface block to a sector at a tile that has already been filled.");
            }

            tile.Ephemeral = block;
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

            if(block.BlockType!=BlockType.Ground)
            {
                _destructibleBlocks.Add(block);
            }
            if (block.Active)
            {
                _activeBlocks.Add(block);
            }
        }

        public void RemoveBlockFromSector(Block block)
        {
            block.Location.ClearBlock(block.BlockType);
            block.Location = null;

            if (block.BlockType != BlockType.Ground)
            {
                _destructibleBlocks.Remove(block);
            }
            if (block.Active)
            {
                _activeBlocks.Remove(block);
            }
        }

        private List<Block> _getBlocksUpdatingOnThisTick(TickCounter tickCounter)
        {
            return _activeBlocks.Where(b => (tickCounter.TotalTicks+b.SpeedOffset) % b.Speed == 0).ToList();
        }
        public List<Block> GetDoomedBlocks()
        {
            return _destructibleBlocks.Where(b => b.ShouldBeDestroyed()).ToList();
        }

        public bool HasNeighbour(CardinalDirection direction)
        {
            return Adjacent.ContainsKey(direction);
        }
    }
}