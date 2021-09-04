using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class BlocksetEditPane_2 : SpriteMenuItem
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

            var plusButton = new SpriteMenuItem(ManualDrawLayer.Create(DrawLayers.MenuBaseLayer), BuiltInMenuSprites.PlusButton);
            plusButton.SetLocationConfig(GetBaseSize().X - 9, 0, CoordinateMode.ParentPixelOffset, false);
            plusButton.OnMouseReleased += (i) => _changeChipScale(2);
            AddChild(plusButton);

            var minusButton = new SpriteMenuItem(ManualDrawLayer.Create(DrawLayers.MenuBaseLayer), BuiltInMenuSprites.MinusButton_Partial);
            minusButton.SetLocationConfig(GetBaseSize().X - 17, 0, CoordinateMode.ParentPixelOffset, false);
            minusButton.OnMouseReleased += (i) => _changeChipScale(0.5f);
            AddChild(minusButton);

            _blocksetScaleProvider = new MenuItemScaleProviderParent_WithMultiplierFetcher(() => _chipScale);
        }


        public void RecieveFromSearchPane(BlockData data, UserInput input)
        {
            var block = _makeAndConfigureNewBlock(data);

            var blockset = _makeBlockset();
            blockset.ScaleProvider = _blocksetScaleProvider;
            blockset.SetInitialDragState(this, input);
            blockset.AddBlock(block, 0);
            AddChildAfterUpdate(blockset);
        }

        public void MakeNewBlocksetWithLiftedBlocks(List<Block_2> blocks, UserInput input)
        {
            blocks.First().ManuallyEndDrag(input);

            var blockset = _makeBlockset();
            blockset.ScaleProvider = _blocksetScaleProvider;
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

            var allHovering = Blocksets.Where(b => b.CanDropThisOn(blockset)).OrderBy(c => c.DrawLayer);
            var hoveringOver = allHovering.FirstOrDefault();
            if(hoveringOver!=null)
            {
                AppendBlockset(blockset, hoveringOver);
                return;
            }

            _alignToPixelGrid(blockset);
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

            _checkForPan(input);
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

        private void _alignToPixelGrid(Blockset_2 blockset)
        {
            var totalDist = blockset.ActualLocation - ActualLocation;
            var trueScale = (int)(Scale * _chipScale);
            var pixelDist = (totalDist / trueScale);
            blockset.SetLocationConfig(pixelDist, CoordinateMode.ParentPixelOffset, false);
        }

        private MenuItemScaleProviderParent_WithMultiplierFetcher _blocksetScaleProvider;
        private float _chipScale = 1.0f;
        private void _changeChipScale(float multiplier)
        {
            var newMultipier = _blocksetScaleProvider.GetScale(this) * multiplier;
            if (newMultipier > 2.995)
            {
                _chipScale *= multiplier;
            }
        }
    }
}
