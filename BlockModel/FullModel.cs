using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class FullModel
    {
        [JsonIgnore]
        public BlocksetModel Initial
        {
            get { return _initial; }
            set
            {
                if(_initial!=null) { _initial.ModeIndex = _getNewModeIndex(); }
                value.ModeIndex = 0;
                _initial = value;
            }
        }
        private BlocksetModel _initial;
        public Dictionary<string,BlocksetModel> Blocksets;
        public Dictionary<string,BlockModel> Blocks;
        [JsonIgnore]
        public Dictionary<BlockInputModel, BlockModel> InputParents;

        public FullModel()
        {
            Blocksets = new Dictionary<string, BlocksetModel>();
            Blocks = new Dictionary<string, BlockModel>();
            InputParents = new Dictionary<BlockInputModel, BlockModel>();
        }

        public BlocksetModel CreateBlockset(string name,bool isInternal)
        {
            var blockset = new BlocksetModel(name,isInternal);
            Blocksets[name] = blockset;

            if(!isInternal)
            {
                blockset.ModeIndex = _getNewModeIndex();
            }

            return blockset;
        }

        public BlockModel CreateBlock(string name,BlockData data)
        {
            var block = new BlockModel(name, data);
            Blocks[name] = block;
            return block;
        }

        public void AddInputs(BlockModel block)
        {
            block.Inputs.ForEach(i => InputParents[i] = block);
        }

        public void DeleteBlockset(BlocksetModel blockset) => Blocksets.Remove(blockset.Name);
        public void DeleteBlocks(List<Block> blocks) => DeleteBlocks(blocks.Select(b => b.Model).ToList());
        public void DeleteBlocks(List<BlockModel> blocks)
        {
            foreach(var b in blocks)
            {
                Blocks.Remove(b.Name);
                b.Inputs.ForEach(i => InputParents.Remove(i));
            }
        }

        public IEnumerable<BlocksetModel> GetModes() => Blocksets.Values.Where(v => !v.Internal);
        private int _getNewModeIndex() => Math.Max(1,Blocksets.Values.Max(v => v.ModeIndex) + 1);

        public string InitialName => Initial.Name;
    }
}
