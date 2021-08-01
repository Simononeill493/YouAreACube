using System;
using System.Collections.Generic;

namespace IAmACube
{
    public class BlockParentCallbacks
    {
        BlockTop _parent;

        public Action<BlockTop, UserInput> BlockLifted;
        public Action<List<BlockTop>, int> AppendBlocks;
        public Action RefreshBlocksetText;

        public Action TopLevelRefreshAll { get { return _topLevelRefreshAll; } 
            set 
            {
                _topLevelRefreshAll = value;
                _parent.GetSubBlocksets().ForEach(c => c.TopLevelRefreshAll = value);
            }
        }
        private Action _topLevelRefreshAll;

        public BlockParentCallbacks(BlockTop parent,Action<BlockTop, UserInput> blockLifted, Action<List<BlockTop>, int> appendBlocks, Action refreshBlocksetText, Action topLevelRefreshAll)
        {
            _parent = parent;

            BlockLifted = blockLifted;
            AppendBlocks = appendBlocks;
            RefreshBlocksetText = refreshBlocksetText;
            TopLevelRefreshAll = topLevelRefreshAll;
        }
    }
}
