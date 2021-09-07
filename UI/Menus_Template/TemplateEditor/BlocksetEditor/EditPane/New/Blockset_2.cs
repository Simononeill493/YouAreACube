using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Blockset_2 : SpriteMenuItem
    {
        public BlocksetModel Model;
        public IEnumerable<Block_2> Blocks => Model.Blocks.Select(b=>BlocksetEditPane_2.Blocks[b]);

        public MenuItem InternalBlocksetBottom;
        public bool IsInternal;
        public bool ShouldDispose => (_empty & !IsInternal) | _manuallyDisposeBlockset;

        public Action<Blockset_2, UserInput> ThisDroppedCallback;
        public Action<List<Block_2>, UserInput> BlockLiftedFromThisCallback;

        public Blockset_2(BlocksetModel model) : base(ManualDrawLayer.Create(DrawLayers.MenuBlockLayer), BuiltInMenuSprites.Blockset_TopHandle)
        {
            Model = model;

            Draggable = true;
            OnEndDrag += (i) => ThisDroppedCallback(this,i);
        }

        public void AddBlock(Block_2 block, int index) => AddBlocks(new List<Block_2>() { block }, index);
        public void AddBlocks(List<Block_2> blocks, int index)
        {
            Model.AddBlocks(blocks, index);
            blocks.ForEach(b => b.SetBlocksetParent(this));

            this.SetBlockPositions();
        }
        public void RemoveBlocks(List<Block_2> toRemove) => Model.RemoveBlocks(toRemove);

        public void LiftBlocks(Block_2 block, UserInput input)
        {
            var toRemove = this.GetThisAndAllBlocksAfter(block);
            RemoveBlocks(toRemove);

            BlockLiftedFromThisCallback(toRemove, input);
        }
        public void DropBlocks(List<Block_2> blocks) => AddBlocks(blocks, _getMouseHoveringIndex());
        public List<Block_2> DetachAllBlocks()
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
        public bool CanDropThisOn(Blockset_2 toDrop) => !Equals(toDrop) && Visible && (_mouseOverAnyBlock | _mouseOverInternalBottom);

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
        





        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            this.SetBlockPositions();
        }

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
        private Block_2 _getMouseHoveringBlock() => Blocks.Where(b => b.MouseHovering).FirstOrDefault();

        private bool _manuallyDisposeBlockset;
        private bool _empty => !Blocks.Any();
        private bool _mouseOverInternalBottom => (InternalBlocksetBottom != null && InternalBlocksetBottom.MouseHovering);
        private bool _mouseOverAnyBlock => Blocks.Any(b => b.MouseHovering);
    }
}
