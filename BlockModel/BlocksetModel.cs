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
        public string Name { get; set; }
        public bool Internal { get; }

        public int ModeIndex = -1;

        public BlocksetModel(string name,bool isInternal)
        {
            Name = name;
            Internal = isInternal;
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
    }
}
