﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class FullModel
    {
        public Dictionary<string,BlocksetModel> Blocksets;
        public Dictionary<string,BlockModel> Blocks;
        public Dictionary<BlockInputModel, BlockModel> InputParents;

        public FullModel()
        {
            Blocksets = new Dictionary<string, BlocksetModel>();
            Blocks = new Dictionary<string, BlockModel>();
            InputParents = new Dictionary<BlockInputModel, BlockModel>();
        }

        public BlocksetModel CreateBlockset(string name)
        {
            var blockset = new BlocksetModel(name);
            Blocksets[name] = blockset;
            return blockset;
        }

        public BlockModel CreateBlock(string name,BlockData data)
        {
            var block = new BlockModel(name, data);
            Blocks[name] = block;

            block.Inputs.ForEach(i => InputParents[i] = block);
            return block;
        }

        public void DeleteBlockset(BlocksetModel blockset) => Blocksets.Remove(blockset.Name);
        public void DeleteBlocks(List<Block_2> blocks) => DeleteBlocks(blocks.Select(b => b.Model).ToList());
        public void DeleteBlocks(List<BlockModel> blocks)
        {
            foreach(var b in blocks)
            {
                Blocks.Remove(b.Name);
                b.Inputs.ForEach(i => InputParents.Remove(i));
            }
        }
    }
}
