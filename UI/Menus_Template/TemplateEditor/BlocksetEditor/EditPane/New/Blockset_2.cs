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
        public List<Block_2> Blocks;

        public BlocksetEditPane_2 Pane;
        public bool Internal;

        public MenuItem InternalBlocksetBottom;

        public Blockset_2(BlocksetModel model) : base(ManualDrawLayer.Create(DrawLayers.MenuBlockLayer), BuiltInMenuSprites.Blockset_TopHandle)
        {
            Model = model;

            Draggable = true;
            OnEndDrag += (i) => Pane.BlocksetDropped(this,i);
            Blocks = new List<Block_2>();
        }

        public void LiftBlocks(Block_2 block, UserInput input)
        {
            var toRemove = this.GetThisAndAllBlocksAfter(block);
            RemoveBlocks(toRemove);

            Pane.MakeNewBlocksetWithLiftedBlocks(toRemove, input);
        }

        public void DropBlocks(List<Block_2> blocks)
        {
            var index = _getIndexMouseIsHoveringOver();
            AddBlocks(blocks, index);
        }

        public void RemoveBlocks(List<Block_2> toRemove)
        {
            RemoveChildrenAfterUpdate(toRemove);
            Model.RemoveBlocks(toRemove);
            Blocks = Blocks.Except(toRemove).ToList();
        }
        public List<Block_2> DetachAllBlocks()
        {
            var blocks = Blocks;
            RemoveBlocks(blocks);
            return blocks;
        }

        public void AddBlock(Block_2 block,int index) => AddBlocks(new List<Block_2>() { block },index);
        public void AddBlocks(List<Block_2> blocks,int index)
        {
            Model.AddBlocks(blocks, index);
            Blocks.InsertRange(index,blocks);
            AddChildren(blocks);

            blocks.ForEach(b => b.Parent = this);
            _setBlockPositions();
        }


        public void SetInitialDragState(MenuItem parent, UserInput input)
        {
            SetLocationConfig(input.MousePos.X - (GetCurrentSize().X / 2), input.MousePos.Y - 2, CoordinateMode.Absolute);
            UpdateLocationCascade(parent);
            TryStartDragAtMousePosition(input);
        }

        public bool CanDropThisOn(Blockset_2 toDrop) => !Equals(toDrop) && Visible && (MouseOverAnyBlock | MouseOverInternalBottom);
        public bool MouseOverInternalBottom => (InternalBlocksetBottom != null && InternalBlocksetBottom.MouseHovering);

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            _setBlockPositions();
        }
        private void _setBlockPositions()
        {
            var offs = IntPoint.Zero;
            offs.Y += GetBaseSize().Y-1;
            foreach(var block in Blocks)
            {
                block.SetLocationConfig(offs, CoordinateMode.ParentPixelOffset);
                offs.Y += block.GetBaseSize().Y;
            }
        }
        public bool Empty => !Blocks.Any();

        private int _getIndexMouseIsHoveringOver()
        {
            if(MouseOverInternalBottom)
            {
                return Blocks.Count();
            }

            var mouseHoveringBlock = Blocks.Where(b => b.MouseHovering).FirstOrDefault();
            var index = this.IndexOf(mouseHoveringBlock);
            if (mouseHoveringBlock.IsHoveringOnBottom)
            {
                index++;
            }

            return index;
        }
        public bool MouseOverAnyBlock => Blocks.Any(b => b.MouseHovering) ;

        public bool ShouldDispose() => (Empty & !Internal) | _manuallyDispose;
        public bool ManuallyDispose() => _manuallyDispose = true;
        private bool _manuallyDispose;

        public override void Dispose()
        {
            base.Dispose();
            if (Blocks.Any())
            {
                throw new Exception("Disposing a blockset which still contains blocks");
            }
        }
    }
}
