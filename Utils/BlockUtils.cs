using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class BlockUtils
    {
        public static IntPoint GetCurrentBlockSize(this Block_2 block)
        {
            IntPoint total= IntPoint.Zero;

            foreach(var section in block.Sections)
            {
                var size = section.GetBaseSize();
                if(size.X>total.X)
                {
                    total.X = size.X;
                }
                total.Y += size.Y-1;
            }

            return total;
        }


        public static List<Block_2> GetThisAndAllBlocksAfter(this Blockset_2 blockset,Block_2 block)
        {
            var blockToRemoveTopIndex = blockset.Blocks.IndexOf(block);
            var numToRemove = blockset.Blocks.Count() - blockToRemoveTopIndex;

            var removed = blockset.Blocks.GetRange(blockToRemoveTopIndex, numToRemove);
            return removed;
        }

        public static int IndexOf(this Blockset_2 blockset, Block_2 block)=> blockset.Blocks.IndexOf(block);
    }
}
