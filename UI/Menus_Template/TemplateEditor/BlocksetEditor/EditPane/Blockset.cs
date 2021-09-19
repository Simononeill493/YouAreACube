using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Blockset : TextBoxMenuItem
    {
        public BlocksetModel Model;
        public bool IsInternal => Model.Internal;

        public IEnumerable<Block> Blocks => Model.Blocks.Select(b=>BlocksetEditPane.Blocks[b]);

        public ScreenItem InternalBlocksetBottom;
        public bool ShouldDispose => (Empty & !IsInternal) | _manuallyDisposeBlockset;
        public bool Empty => !Blocks.Any();

        public Action<Blockset, UserInput> ThisDroppedCallback;
        public Action<Blockset,List<Block>, UserInput> BlocksLiftedFromThisCallback;
        public Action<BlockInputOption, BlockInputModel> OpenSubMenuCallback;

        public Blockset(BlocksetModel model) : base(ManualDrawLayer.Create(DrawLayers.MenuBlockLayer), ()=>model.Name,(s)=>model.Name=s)
        {
            Model = model;
            SpriteName = MenuSprites.Blockset_TopHandle;

            Editable = true;
            MaxTextLength = 50;
            _textItem.MultiplyScale(0.5f);

            if(!IsInternal)
            {
                Draggable = true;
                OnEndDrag += (i) => ThisDroppedCallback(this, i);

                var modeIndexText = _addTextItem(() => Model.ModeIndex.ToString(), 4, 50, CoordinateMode.ParentPercentage, true);
                modeIndexText.MultiplyScale(0.5f);

                var initalCheckBox = new CheckBoxMenuItem(this, () => model.Equals(BlocksetEditPane.Model.Initial), (b) => _tryMakeInitial(model, b));
                initalCheckBox.Size = new IntPoint(8, 8);
                initalCheckBox.SetLocationConfig(96, 50, CoordinateMode.ParentPercentage, true);
                AddChild(initalCheckBox);
            }
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

            BlocksLiftedFromThisCallback(this,toRemove, input);
        }
        public void DropBlocks(List<Block> blocks) => AddBlocks(blocks, _getMouseHoveringIndex());
        public List<Block> DetachAllBlocks()
        {
            var blocks = Blocks.ToList();
            RemoveBlocks(blocks);
            return blocks;
        }

        public void SetInitialDragState(ScreenItem parent, UserInput input)
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
        private bool _mouseOverInternalBottom => (InternalBlocksetBottom != null && InternalBlocksetBottom.MouseHovering);
        private bool _mouseOverAnyBlock => Blocks.Any(b => b.MouseHovering);


        private static void _tryMakeInitial(BlocksetModel model, bool isTrue)
        {
            if (isTrue)
            {
                BlocksetEditPane.Model.Initial = model;
            }
        }
    }
}
