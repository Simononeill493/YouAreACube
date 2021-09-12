using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Blockset : SpriteMenuItem
    {
        public BlocksetModel Model;
        public IEnumerable<Block> Blocks => Model.Blocks.Select(b=>BlocksetEditPane.Blocks[b]);

        public MenuItem InternalBlocksetBottom;
        public bool IsInternal;
        public bool ShouldDispose => (_empty & !IsInternal) | _manuallyDisposeBlockset;

        public Action<Blockset, UserInput> ThisDroppedCallback;
        public Action<List<Block>, UserInput> BlockLiftedFromThisCallback;
        public Action<BlockInputOption, BlockInputModel> OpenSubMenuCallback;

        public Blockset(BlocksetModel model) : base(ManualDrawLayer.Create(DrawLayers.MenuBlockLayer), BuiltInMenuSprites.Blockset_TopHandle)
        {
            Model = model;

            Draggable = true;
            OnEndDrag += (i) => ThisDroppedCallback(this,i);
        }


        public void AddBlock(Block block, int index) => AddBlocks(new List<Block>() { block }, index);
        public void AddBlocks(List<Block> blocks, int index)
        {
            Model.AddBlocks(blocks, index);

            this.SetBlockPositions();
        }
        public void RemoveBlocks(List<Block> toRemove) => Model.RemoveBlocks(toRemove);

        public void LiftBlocks(Block block, UserInput input)
        {
            var toRemove = this.GetThisAndAllBlocksAfter(block);
            RemoveBlocks(toRemove);

            BlockLiftedFromThisCallback(toRemove, input);
        }
        public void DropBlocks(List<Block> blocks) => AddBlocks(blocks, _getMouseHoveringIndex());
        public List<Block> DetachAllBlocks()
        {
            var blocks = Blocks.ToList();
            RemoveBlocks(blocks);
            return blocks;
        }

        public void SetInitialDragState(MenuItem parent, UserInput input)
        {
            SetLocationConfig(input.MousePos.X - (GetCurrentSize().X / 2), input.MousePos.Y - 2, CoordinateMode.Absolute);
            UpdateLocationCascade(parent);
            TryStartDragAtMousePosition(input);
        }
        public bool CanDropThisOn(Blockset toDrop) => !Equals(toDrop) && Visible && (_mouseOverAnyBlock | _mouseOverInternalBottom);

        public bool ManuallyDispose() => _manuallyDisposeBlockset = true;
        public override void Dispose()
        {
            base.Dispose();
            if (Blocks.Any())
            {
                throw new Exception("Disposing a blockset which still contains blocks");
            }
        }

        public override void HideAndDisable() 
        {
            base.HideAndDisable();
            Blocks.ToList().ForEach(b => b.HideAndDisable());
        }
        public override void ShowAndEnable()
        {
            base.ShowAndEnable();
            Blocks.ToList().ForEach(b => b.ShowAndEnable());
        }

        public void OpenSubMenu(BlockInputOption option, BlockInputModel model) => OpenSubMenuCallback(option, model);






        protected override bool _canStartDragging() => base._canStartDragging() & !IsInternal;

        private int _getMouseHoveringIndex()
        {
            if (_mouseOverInternalBottom)
            {
                return Blocks.Count();
            }

            var mouseHoveringBlock = _getMouseHoveringBlock();
            var index = this.IndexOf(mouseHoveringBlock);
            if (mouseHoveringBlock.IsHoveringOnBottom)
            {
                index++;
            }

            return index;
        }
        private Block _getMouseHoveringBlock() => Blocks.Where(b => b.MouseHovering).FirstOrDefault();

        private bool _manuallyDisposeBlockset;
        private bool _empty => !Blocks.Any();
        private bool _mouseOverInternalBottom => (InternalBlocksetBottom != null && InternalBlocksetBottom.MouseHovering);
        private bool _mouseOverAnyBlock => Blocks.Any(b => b.MouseHovering);
    }
}
