using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class BlockPositioningUtils
    {
        public static IntPoint GetCurrentBlockSize(this Block block)
        {
            IntPoint total = IntPoint.Zero;

            foreach (var section in block.Sections)
            {
                var size = section.GetBaseSize();
                if (size.X > total.X)
                {
                    total.X = size.X;
                }
                total.Y += size.Y - 1;
            }

            return total;
        }

        public static IntPoint GetSizeIncludingBlocks(this Blockset blockset)
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

        public static IntPoint GetSizeWithSubBlockset(this BlockSwitchSection switchSection)
        {
            IntPoint total = switchSection.GetSizeWithoutSubBlockset();

            if (switchSection.ActiveSection != null)
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

        public static  void SetBlockPositions(this Blockset blockset)
        {
            var offs = IntPoint.Zero;
            offs.Y += blockset.GetBaseSize().Y - 1;
            foreach (var block in blockset.Blocks)
            {
                block.SetBlocksetParent(blockset);
                block.SetLocationConfig(offs, CoordinateMode.VisualParentPixelOffset);
                offs.Y += block.GetBaseSize().Y;
            }
        }

        public static void SetSubBlocksetPosition(this BlockSwitchSection switchSection)
        {
            if(switchSection.IsAnySectionActivated)
            {
                var sizeY = switchSection.GetSizeWithoutSubBlockset().Y - 1;
                switchSection.ActiveSection.SetLocationConfig(0, sizeY, CoordinateMode.VisualParentPixelOffset);

                var blocksetY = switchSection.ActiveSection.GetSizeIncludingBlocks().Y - 1;
                switchSection.SwitchSectionBottom.SetLocationConfig(0, sizeY + blocksetY, CoordinateMode.ParentPixelOffset);
            }
        }

        public static void SetPositions(this BlocksetEditPane pane)
        {
            foreach (var blockset in BlocksetEditPane.Blocksets.Values)
            {
                if(blockset.Visible)
                {
                    blockset.SetBlockPositions();
                }
            }
            foreach (var block in BlocksetEditPane.Blocks.Values)
            {
                if(block.Visible)
                {
                    block.SwitchSection?.SetSubBlocksetPosition();
                }
            }
        }
    }
}
