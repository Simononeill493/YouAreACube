using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlocksetModel
    {
        public string Name;

        public BlocksetModel(string name)
        {
            Name = name;
        }

        [JsonIgnore]
        public List<BlockModel> Blocks = new List<BlockModel>();

        public void AddBlocks(List<Block> toAdd, int index) => AddBlocks(toAdd.Select(b => b.Model).ToList(),index);
        public void AddBlocks(List<BlockModel> toAdd, int index)
        {
            Blocks.InsertRange(index, toAdd);
        }

        public void RemoveBlocks(List<Block> toRemove) => RemoveBlocks(toRemove.Select(b => b.Model).ToList());
        public void RemoveBlocks(List<BlockModel> toRemove)
        {
            Blocks = Blocks.Except(toRemove).ToList();
        }

        public List<string> BlockNames => Blocks.Select(b=>b.Name).ToList();
        //public List<BlocksetModel> GetSubBlocksets() => Blocks.SelectMany(b => b.SubBlocksets.Select(s=>s.Item2)).ToList();
    }
}
