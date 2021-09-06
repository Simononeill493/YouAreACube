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
            IntPoint total = IntPoint.Zero;

            foreach (var section in block.Sections)
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

        public static IntPoint GetSizeIncludingBlocks(this Blockset_2 blockset)
        {
            IntPoint total = blockset.GetBaseSize();

            foreach (var block in blockset.Blocks)
            {
                var size = block.GetBaseSize();
                if (size.X > total.X)
                {
                    total.X = size.X;
                }
                total.Y += size.Y - 1;
            }

            return total;
        }

        public static IntPoint GetSizeWithSubBlockset(this BlockSwitchSection_2 switchSection)
        {
            IntPoint total = switchSection.GetSizeWithoutSubBlockset();

            if(switchSection.ActiveSection!=null)
            {
                var size = switchSection.ActiveSection.GetSizeIncludingBlocks();
                if (size.X > total.X)
                {
                    total.X = size.X;
                }
                total.Y += size.Y - 1;

                var bottomSize = switchSection.SwitchSectionBottom.GetBaseSize();
                if (bottomSize.X > total.X)
                {
                    total.X = bottomSize.X;
                }

                total.Y += bottomSize.Y - 1;
            }
            return total;
        }

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
