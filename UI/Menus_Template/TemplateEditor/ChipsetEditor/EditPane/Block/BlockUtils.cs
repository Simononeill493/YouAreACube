using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class BlockUtils
    {
        public static List<Block> GetThisAndAllBlocksAfter(this Blockset blockset,Block block)
        {
            var blocks = blockset.Blocks.ToList();
            var blockToRemoveTopIndex = blocks.IndexOf(block);
            var numToRemove = blockset.Blocks.Count() - blockToRemoveTopIndex;

            var removed = blocks.GetRange(blockToRemoveTopIndex, numToRemove);
            return removed;
        }

        public static int IndexOf(this Blockset blockset, Block block)=> blockset.Blocks.ToList().IndexOf(block);


        public static int GetDepth(this Dictionary<BlocksetModel, Blockset> allBlocksets, Blockset toFind)
        {
            var nonInternals = allBlocksets.Values.Where(b => !b.IsInternal);
            var depth = _getDepth(allBlocksets, nonInternals, toFind, 0);
            return depth;
        }

        private static int _getDepth(Dictionary<BlocksetModel, Blockset> allBlocksets,IEnumerable<Blockset> blocksetsToSearch, Blockset toFind, int currentDepth)
        {
            foreach(var blockset in blocksetsToSearch)
            {
                if(toFind.Equals(blockset))
                {
                    return currentDepth;
                }

                foreach(var subBlocksets in blockset.Blocks.Select(b=>b.Model.SubBlocksets.Select(s=>allBlocksets[s.Item2])))
                {
                    var depth = _getDepth(allBlocksets, subBlocksets, toFind, currentDepth + 1);
                    if(depth>=0)
                    {
                        return depth;
                    }
                }
            }

            return -1;
        }
    }
}
