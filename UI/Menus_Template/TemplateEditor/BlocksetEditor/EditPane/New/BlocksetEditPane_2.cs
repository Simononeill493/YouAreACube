using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlocksetEditPane_2 : SpriteMenuItem
    {
        public static IVariableProvider VariableProvider;
        public static FullModel Model;

        public List<Blockset_2> Blocksets;
        private MenuItem _bin;

        public BlocksetEditPane_2(IVariableProvider variableProvider,MenuItem bin) : base(ManualDrawLayer.Create(DrawLayers.BackgroundLayer),BuiltInMenuSprites.BlocksetEditPane)
        {
            VariableProvider = variableProvider;
            _bin = bin;

            Model = new FullModel();
            Blocksets = new List<Blockset_2>();
        }

        public void RecieveFromSearchPane(BlockData data, UserInput input)
        {
            var block = _makeAndConfigureNewBlock(data);

            var blockset = _makeBlockset();
            blockset.SetInitialDragState(this, input);
            blockset.AddBlock(block, 0);
            AddChildAfterUpdate(blockset);
        }

        public void MakeNewBlocksetWithLiftedBlocks(List<Block_2> blocks, UserInput input)
        {
            blocks.First().ManuallyEndDrag(input);

            var blockset = _makeBlockset();
            blockset.SetInitialDragState(this, input);
            blockset.AddBlocks(blocks, 0);
            AddChildAfterUpdate(blockset);
        }

        public void AppendBlockset(Blockset_2 toAppend, Blockset_2 target)
        {
            var blocks = toAppend.DetachAllBlocks();
            target.DropBlocks(blocks);
        }

        public void BlocksetDropped(Blockset_2 blockset, UserInput input)
        {
            if(_bin.MouseHovering)
            {
                blockset.ManuallyDispose();
                return;
            }

            var hoveringOver = Blocksets.Where(b => b.CanDropThisOn(blockset));
            if(hoveringOver.Count()>1)
            {
                var overBottom = hoveringOver.Where(h => h.MouseOverInternalBottom);
                if(overBottom.Count()!=1)
                {

                }
                else
                {
                    AppendBlockset(blockset, overBottom.First());
                }
            }
            if(hoveringOver.Any())
            {
                AppendBlockset(blockset, hoveringOver.First());
            }
        }





        public void LoadChipsetForEditing(CubeTemplate template) 
        {

        }
        public void AddEditedChipsetToTemplate(CubeTemplate template) 
        { 

        }

        private Block_2 _makeAndConfigureNewBlock(BlockData data)
        {
            var newModel = Model.CreateBlock(IDUtils.GenerateBlockID(data), data);
            var block = BlockFactory.MakeBlock(data, newModel, _makeBlockset);
            block.SwitchSection?.GenerateDefaultSections();
            return block;
        }

        private Blockset_2 _makeBlockset()
        {
            var newModel = Model.CreateBlockset(IDUtils.GenerateBlocksetID());
            var blockset = BlockFactory.MakeBlockset(newModel);

            blockset.Pane = this;
            Blocksets.Add(blockset);
            return blockset;
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            var toDispose = Blocksets.Where(b => b.ShouldDispose()).ToList();
            toDispose.ForEach(b => _disposeBlockset(b));
        }

        private void _disposeBlockset(Blockset_2 blockset)
        {
            Model.DeleteBlockset(blockset.Model);
            if (!blockset.Internal) { RemoveChild(blockset); }
            Blocksets.Remove(blockset);

            var blocks = blockset.DetachAllBlocks();
            _disposeBlocks(blocks);
            blockset.Dispose();
        }

        private void _disposeBlocks(List<Block_2> blocks)
        {
            Model.DeleteBlocks(blocks);
            foreach (var block in blocks)
            {
                block.GetSubBlocksets().ForEach(s => _disposeBlockset(s));
                block.Dispose();
            }
        }
    }
}
