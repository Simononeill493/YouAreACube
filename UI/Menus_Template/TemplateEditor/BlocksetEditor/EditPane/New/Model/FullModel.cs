using System;
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

        public FullModel()
        {
            Blocksets = new Dictionary<string, BlocksetModel>();
            Blocks = new Dictionary<string, BlockModel>();
        }

        public BlocksetModel MakeBlockset(string name)
        {
            var blockset = new BlocksetModel(name);
            Blocksets[name] = blockset;
            return blockset;
        }

        public BlockModel MakeBlock(string name,BlockData data)
        {
            var block = new BlockModel(name, data);
            Blocks[name] = block;
            return block;
        }

        public void RemoveBlockset(BlocksetModel blockset) => Blocksets.Remove(blockset.Name);
        public void RemoveBlocks(List<Block_2> blocks) => RemoveBlocks(blocks.Select(b => b.Model).ToList());
        public void RemoveBlocks(List<BlockModel> blocks) => blocks.ForEach(b => Blocks.Remove(b.Name));
    }
}
