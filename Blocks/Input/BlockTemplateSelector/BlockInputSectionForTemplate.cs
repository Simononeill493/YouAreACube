using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputSectionForTemplate: BlockInputSection
    {
        public BlockInputSectionForTemplate(IHasDrawLayer parent, string inputDisplayName) : base(parent,new List<string> { "BlockTemplate" }, "ChipFullMiddle")
        {

        }

        public override BlockInputOption CurrentlySelected => throw new NotImplementedException();

        protected override void _manuallySetInput(BlockInputOption option)
        {
            throw new NotImplementedException();
        }

        public override void RefreshText()
        {
            throw new NotImplementedException();
        }

        public override void SetConnectionsFromAbove(List<BlockTop> chipsAbove)
        {
            throw new NotImplementedException();
        }
    }
}
