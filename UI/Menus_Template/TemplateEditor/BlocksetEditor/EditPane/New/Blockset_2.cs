using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Blockset_2 : SpriteMenuItem
    {
        public string Name => Model.Name;
        public BlocksetModel Model;

        public bool Internal;

        public List<Block_2> Blocks;
        public BlocksetEditPane_2 Pane;

        public Blockset_2(BlocksetModel model) : base(ManualDrawLayer.Create(DrawLayers.MenuBlockLayer), BuiltInMenuSprites.Blockset_TopHandle)
        {
            Model = model;

            Draggable = true;
            OnEndDrag += (i) => Pane.BlocksetDropped(this,i);
            Blocks = new List<Block_2>();
        }

        public void SetInitialDragState(MenuItem parent, UserInput input)
        {
            SetLocationConfig(input.MousePos.X - (GetCurrentSize().X / 2), input.MousePos.Y - 2, CoordinateMode.Absolute);
            UpdateDimensionsCascade(parent);
            TryStartDragAtMousePosition(input);
        }

        public void BlockLifted(Block_2 block, UserInput input)
        {
            var toRemove = this.GetThisAndAllBlocksAfter(block);
            RemoveBlocks(toRemove);

            Pane.MakeNewBlocksetWithLiftedBlocks(toRemove, input);
        }

        public void RemoveBlocks(List<Block_2> toRemove)
        {
            Model.RemoveBlocks(toRemove);
            Blocks = Blocks.Except(toRemove).ToList();
            RemoveChildrenAfterUpdate(toRemove);

            Pane.BlocksRemovedFromBlockset(this, toRemove);
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


        public void Drop(List<Block_2> blocks)
        {
            var mouseHoveringBlock = Blocks.Where(b => b.MouseHovering).FirstOrDefault();
            var index = this.IndexOf(mouseHoveringBlock);
            if(mouseHoveringBlock.IsHoveringOnBottom)
            {
                index++;
            }

            AddBlocks(blocks, index);
        }

        public override void Update(UserInput input)
        {
            _setBlockPositions();
            base.Update(input);
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
        public bool MouseOverAnyBlock => Blocks.Any(b => b.MouseHovering);
    }
}
