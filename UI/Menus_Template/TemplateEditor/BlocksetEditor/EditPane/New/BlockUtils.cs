using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class BlockUtils
    {


        public static List<Block_2> GetThisAndAllBlocksAfter(this Blockset_2 blockset,Block_2 block)
        {
            var blocks = blockset.Blocks.ToList();
            var blockToRemoveTopIndex = blocks.IndexOf(block);
            var numToRemove = blockset.Blocks.Count() - blockToRemoveTopIndex;

            var removed = blocks.GetRange(blockToRemoveTopIndex, numToRemove);
            return removed;
        }

        public static int IndexOf(this Blockset_2 blockset, Block_2 block)=> blockset.Blocks.ToList().IndexOf(block);
    }
}
