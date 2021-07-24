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
        private IBlocksetContainer _currentContainer;

        public Blockset(string name,IHasDrawLayer parent,float scaleMultiplier,Action<List<BlockTop>,UserInput,Blockset> liftBlocksCallback) : base(parent, "TopOfChipset")
        {
            Name = name;
            Blocks = new List<BlockTop>();
            MultiplyScaleCascade(scaleMultiplier);

            _liftBlocksCallback = liftBlocksCallback;
            HeightOfAllBlocks += GetBaseSize().Y;
        }


        public void AppendBlockToTop(BlockTop blockToDrop) => AppendBlocksToTop(new List<BlockTop>() { blockToDrop });
        public void AppendBlocksToTop(List<BlockTop> blocksToDrop) => AppendBlocks(blocksToDrop, 0);

        public void AppendBlockToBottom(BlockTop chipToDrop) => AppendBlocksToBottom(new List<BlockTop>() { chipToDrop });
        public void AppendBlocksToBottom(List<BlockTop> chipsToDrop) => AppendBlocks(chipsToDrop, Blocks.Count);

        public void DropBlocksOn(List<BlockTop> blocksToDrop, UserInput input)
        {
            var itemToDropOn = _getChipsetSectionMouseIsOver();

            if(itemToDropOn==null)
            {
                Console.WriteLine("Warning: dropped chips onto a chipset that was supposed to be under the mouse, but wasn't");
                return;
            }
            else if(itemToDropOn==this)
            {
                AppendBlocksToTop(blocksToDrop);
            }
            else
            {
                itemToDropOn.DropBlocksOn(blocksToDrop, input);
            }
        }

        public void AppendBlocks(List<BlockTop> toAdd, int index)
        {
            Blocks.InsertRange(index, toAdd);
            AddChildren(toAdd);

            foreach (var chip in toAdd)
            {
                chip.UpdateDrawLayerCascade(DrawLayer);
                chip.ChipLiftedCallback = _chipLiftedFromChipset;
                chip.AppendChips = AppendBlocks;
                chip.ChipsetRefreshText = RefreshText;
                chip.TopLevelRefreshAll = TopLevelRefreshAll;
            }

            TopLevelRefreshAll();
        }
        public List<BlockTop> PopChips(int index)
        {
            var toRemove = Blocks.Skip(index).ToList();
            Blocks.RemoveRange(index, toRemove.Count());
            RemoveChildrenAfterUpdate(toRemove);

            TopLevelRefreshAll();
            return toRemove;
        }


        public void SetContainer(IBlocksetContainer newContainer)
        {
            if (newContainer == _currentContainer) { return; }

            ClearContainer();

            newContainer.AddBlockset(this);
            _currentContainer = newContainer;
        }
        public void ClearContainer()
        {
            if (_currentContainer != null)
            {
                _currentContainer.RemoveBlockset(this);
            }

            _currentContainer = null;
        }
        public override void Dispose()
        {
            if (_currentContainer != null)
            {
                throw new Exception("Tried to dispose a chipset that is still contained!");
            }

            base.Dispose();
        }


        public bool IsMouseOverAnyChip() => _getChipsetSectionMouseIsOver() != null;

        public List<Blockset> GetThisAndSubChipsets()
        {
            var sub = GetSubChipsets();
            sub.Add(this);
            return sub;
        }
        public List<Blockset> GetSubChipsets()
        {
            var output = new List<Blockset>();
            var subChipsets = Blocks.Select(c => c.GetSubChipsets());

            foreach (var sublist in subChipsets)
            {
                foreach(var sub in sublist)
                {
                    output.AddRange(sub.GetThisAndSubChipsets());
                }
            }

            return output;
        }


        private void _chipLiftedFromChipset(BlockTop chip, UserInput input)
        {
            var chipsRemoved = PopChips(chip.IndexInChipset);
            _liftBlocksCallback(chipsRemoved, input, this);
        }
        private IBlocksDroppableOn _getChipsetSectionMouseIsOver()
        {
            if (MouseHovering)
            {
                return this;
            }

            foreach (var chip in Blocks)
            {
                if (chip.IsMouseOverAnySection())
                {
                    return chip;
                }
            }

            return null;
        }

        public override string ToString() => Name;
    }
}
