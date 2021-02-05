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
        public Tile[,] Tiles;
        public List<Tile> TilesFlattened;
        public IEnumerable<Block> DoomedBlocks => _destructibleBlocks.Where(b => b.ShouldBeDestroyed());

        private List<Block> _activeBlocks;
        private List<Block> _destructibleBlocks;

        public Sector(Point location,Tile[,] tiles, List<Tile> tilesFlattened) : base(location)
        {
            Tiles = tiles;
            TilesFlattened = tilesFlattened;

            _activeBlocks = new List<Block>();
            _destructibleBlocks = new List<Block>();
        }

        public ActionsList UpdateBlocks(UserInput input,TickManager tickCounter)
        {
            var actions = new ActionsList();
            var toUpdate = tickCounter.GetUpdatingBlocks(_activeBlocks);

            foreach(var block in toUpdate)
            {
                block.Update(input,actions);
            }

            return actions;
        }

        public Tile GetTile(Point point)
        {
            var tile = Tiles[point.X, point.Y];
            return tile;
        }

        public void AddToSector(Block block, Tile tile)
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
        private void _addToSector(Block block, Tile tile)
        {
            block.Location = tile;
            //todo ensure EVERY time a block is added that this is set 

            if (block.BlockType != BlockType.Ground)
            {
                _destructibleBlocks.Add(block);
            }
            if (block.Active)
            {
                _activeBlocks.Add(block);
            }
        }
        public void RemoveFromSector(Block block)
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

        public void AddSurfaceToSector(SurfaceBlock block,Tile tile)
        {
            if(tile.HasSurface)
            {
                Console.WriteLine("Warning: adding a surface block to a sector at a tile that has already been filled.");
            }

            tile.Surface = block;
            _addToSector(block, tile);
        }
        public void AddEphemeralToSector(EphemeralBlock block, Tile tile)
        {
            if (tile.HasEphemeral)
            {
                Console.WriteLine("Warning: adding a surface block to a sector at a tile that has already been filled.");
            }

            tile.Ephemeral = block;
            _addToSector(block, tile);
        }
        public void AddGroundToSector(GroundBlock block, Tile tile)
        {
            if (tile.Ground != null)
            {
                Console.WriteLine("Warning: adding ground to a sector at a tile that has already been filled.");
            }

            tile.Ground= block;
            _addToSector(block, tile);
        }
    }
}