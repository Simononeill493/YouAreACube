using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlocksetEditPane_2 : SpriteMenuItem
    {
        public List<Blockset_2> Blocksets;

        public BlocksetEditPane_2() : base(ManualDrawLayer.Create(DrawLayers.BackgroundLayer),BuiltInMenuSprites.BlocksetEditPane)
        {
            Blocksets = new List<Blockset_2>();
        }

        public void BlocksLiftedFromBlockset(Blockset_2 blockset,List<Block_2> blocks, UserInput input)
        {
            if (blockset.Empty & !blockset.Internal)
            {
                _disposeBlockset(blockset);
            }

            blocks.First().ManuallyEndDrag(input);

            var newBlockset = _makeBlockset();
            newBlockset.SetInitialDragState(this, input);
            newBlockset.AddBlocks(blocks,0);
        }

        public void BlocksetDropped(Blockset_2 blockset, UserInput input)
        {
            var hoveringOver = Blocksets.Where(b => !b.Equals(blockset) && b.MouseOverAnyBlock && b.Visible);
            if(hoveringOver.Count()>1)
            {

            }
            if(hoveringOver!=null)
            {
                AppendBlockset(blockset, hoveringOver.First());
            }
        }

        public void AppendBlockset(Blockset_2 toAppend,Blockset_2 target)
        {
            target.Drop(toAppend.Blocks);
            _disposeBlockset(toAppend);
        }

        public void RecieveFromSearchPane(BlockData data, UserInput input)
        {
            var block = BlockFactory.MakeBlock(this, data);
        
            var blockset = _makeBlockset();
            blockset.SetInitialDragState(this,input);
            blockset.AddBlock(block,0);
        }

        private Blockset_2 _makeBlockset()
        {
            var blockset = BlockFactory.MakeBlockset(this);
            blockset.Pane = this;
            AddChildAfterUpdate(blockset);
            Blocksets.Add(blockset);
            return blockset;
        }

        private void _disposeBlockset(Blockset_2 blockset)
        {
            RemoveChildAfterUpdate(blockset);
            Blocksets.Remove(blockset);
            blockset.Dispose();
        }


        public void LoadChipsetForEditing(CubeTemplate template) 
        {

        }
        public void AddEditedChipsetToTemplate(CubeTemplate template) 
        { 

        }
    }
}
