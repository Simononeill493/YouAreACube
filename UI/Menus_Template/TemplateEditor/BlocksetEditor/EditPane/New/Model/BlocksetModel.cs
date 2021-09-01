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

        public List<BlockModel> Blocks = new List<BlockModel>();

        public void AddBlocks(List<Block_2> toAdd, int index) => AddBlocks(toAdd.Select(b => b.Model).ToList(),index);
        public void AddBlocks(List<BlockModel> toAdd, int index)
        {
            Blocks.InsertRange(index, toAdd);
        }

        public void RemoveBlocks(List<Block_2> toRemove) => RemoveBlocks(toRemove.Select(b => b.Model).ToList());
        public void RemoveBlocks(List<BlockModel> toRemove)
        {
            Blocks = Blocks.Except(toRemove).ToList();
        }
    }
}
