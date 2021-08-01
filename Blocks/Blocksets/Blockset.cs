using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class Blockset : DraggableMenuItem, IBlocksDroppableOn
    {
        public string Name;
        public List<BlockTop> Blocks { get; private set; }

        private Action<List<BlockTop>, UserInput, Blockset> _liftBlocksCallback;
        public int HeightOfAllBlocks;

        public Blockset(string name,IHasDrawLayer parent,float scaleMultiplier,Action<List<BlockTop>,UserInput,Blockset> liftBlocksCallback) : base(parent, "TopOfChipset")
        {
            Name = name;
            Blocks = new List<BlockTop>();
            MultiplyScaleCascade(scaleMultiplier);

            _liftBlocksCallback = liftBlocksCallback;
            HeightOfAllBlocks += GetBaseSize().Y;
        }


        public void AppendBlockToTop(BlockTop blockToDrop) => AppendBlocksToTop(new List<BlockTop>() { blockToDrop });
        public void AppendBlocksToTop(List<BlockTop> blocksToDrop) => _appendBlocks(blocksToDrop, 0);

        public void AppendBlockToBottom(BlockTop blockToDrop) => AppendBlocksToBottom(new List<BlockTop>() { blockToDrop });
        public void AppendBlocksToBottom(List<BlockTop> blocksToDrop) => _appendBlocks(blocksToDrop, Blocks.Count);

        public void DropBlocksOnThis(List<BlockTop> blocksToDrop, UserInput input)
        {
            var itemToDropOn = _getBlocksetSectionMouseIsOver();

            if(itemToDropOn==null)
            {
                Console.WriteLine("Warning: dropped blocks onto a blockset that was supposed to be under the mouse, but wasn't");
                return;
            }
            else if(itemToDropOn==this)
            {
                AppendBlocksToTop(blocksToDrop);
            }
            else
            {
                itemToDropOn.DropBlocksOnThis(blocksToDrop, input);
            }
        }

        private void _appendBlocks(List<BlockTop> toAdd, int index)
        {
            Blocks.InsertRange(index, toAdd);
            AddChildren(toAdd);
            toAdd.ForEach(b => _setParentDataForBlock(b));

            TopLevelRefreshAll();
        }
        private void _setParentDataForBlock(BlockTop block)
        {
            block.UpdateDrawLayerCascade(DrawLayer);
            block.Callbacks = new BlockParentCallbacks(block,_blockLiftedFromBlockset, _appendBlocks, RefreshText, TopLevelRefreshAll);
        }

        public List<BlockTop> PopBlocks(int index)
        {
            var toRemove = Blocks.Skip(index).ToList();
            Blocks.RemoveRange(index, toRemove.Count());
            RemoveChildrenAfterUpdate(toRemove);

            TopLevelRefreshAll();
            return toRemove;
        }

        public bool IsMouseOverAnyBlock() => _getBlocksetSectionMouseIsOver() != null;

        public List<Blockset> GetThisAndSubBlocksets()
        {
            var sub = GetSubBlocksets();
            sub.Add(this);
            return sub;
        }
        public List<Blockset> GetSubBlocksets()
        {
            var output = new List<Blockset>();
            var subBlocksets = Blocks.Select(c => c.GetSubBlocksets());

            foreach (var sublist in subBlocksets)
            {
                foreach(var sub in sublist)
                {
                    output.AddRange(sub.GetThisAndSubBlocksets());
                }
            }

            return output;
        }



        private void _blockLiftedFromBlockset(BlockTop block, UserInput input)
        {
            var blocksRemoved = PopBlocks(block.IndexInBlockset);
            _liftBlocksCallback(blocksRemoved, input, this);
        }
        private IBlocksDroppableOn _getBlocksetSectionMouseIsOver()
        {
            if (MouseHovering)
            {
                return this;
            }

            foreach (var block in Blocks)
            {
                if (block.IsMouseOverAnySection())
                {
                    return block;
                }
            }

            return null;
        }

        public override string ToString() => Name;
    }
}
