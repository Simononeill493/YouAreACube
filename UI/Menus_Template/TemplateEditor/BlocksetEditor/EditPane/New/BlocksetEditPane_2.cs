using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class BlocksetEditPane_2 : SpriteMenuItem
    {
        public static Dictionary<BlocksetModel, Blockset_2> Blocksets;
        public static Dictionary<BlockModel, Block_2> Blocks;

        public static FullModel Model;
        public static IVariableProvider VariableProvider;

        private MenuItem _bin;

        public BlocksetEditPane_2(IVariableProvider variableProvider,MenuItem bin) : base(ManualDrawLayer.Create(DrawLayers.BackgroundLayer),BuiltInMenuSprites.BlocksetEditPane)
        {
            VariableProvider = variableProvider;
            _bin = bin;

            Model = new FullModel();
            Blocksets = new Dictionary<BlocksetModel, Blockset_2>();
            Blocks = new Dictionary<BlockModel, Block_2>();

            var plusButton = new SpriteMenuItem(ManualDrawLayer.Create(DrawLayers.MenuBaseLayer), BuiltInMenuSprites.PlusButton);
            plusButton.SetLocationConfig(GetBaseSize().X - 9, 0, CoordinateMode.ParentPixelOffset, false);
            plusButton.OnMouseReleased += (i) => _changeChipScale(2);
            AddChild(plusButton);

            var minusButton = new SpriteMenuItem(ManualDrawLayer.Create(DrawLayers.MenuBaseLayer), BuiltInMenuSprites.MinusButton_Partial);
            minusButton.SetLocationConfig(GetBaseSize().X - 17, 0, CoordinateMode.ParentPixelOffset, false);
            minusButton.OnMouseReleased += (i) => _changeChipScale(0.5f);
            AddChild(minusButton);

            _blocksetScaleProvider = new MenuItemScaleProviderParent();
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            _cleanUpDisposedBlocksets();
            _checkForPan(input);
            _correctScale();
        }

        public void RecieveFromSearchPane(BlockData data, UserInput input)
        {
            var block = _makeNewBlock(data);

            var blockset = _makeNewBlockset();
            blockset.SetInitialDragState(this, input);
            blockset.AddBlock(block, 0);
        }

        public void MakeNewBlocksetWithLiftedBlocks(List<Block_2> blocks, UserInput input)
        {
            blocks.First().ManuallyEndDrag(input);

            var blockset = _makeNewBlockset();
            blockset.SetInitialDragState(this, input);
            blockset.AddBlocks(blocks, 0);
        }

        public void BlocksetDropped(Blockset_2 blockset, UserInput input)
        {
            if(_bin.MouseHovering)
            {
                blockset.ManuallyDispose();
                return;
            }

            var hoveringOver = _getBlocksetMouseIsOver(blockset);
            if(hoveringOver!=null)
            {
                AppendBlockset(blockset, hoveringOver);
                return;
            }

            _alignToPixelGrid(blockset);
        }

        public void AppendBlockset(Blockset_2 toAppend, Blockset_2 target) => target.DropBlocks(toAppend.DetachAllBlocks());




        public void LoadChipsetForEditing(CubeTemplate template) 
        {

        }

        public void AddEditedChipsetToTemplate(CubeTemplate template) 
        { 

        }



        private Block_2 _makeNewBlock(BlockData data)
        {
            var newModel = Model.CreateBlock(IDUtils.GenerateBlockID(data), data);
            return _makeBlockFromModel(newModel);
        }

        private Blockset_2 _makeNewBlockset()
        {
            var newModel = Model.CreateBlockset(IDUtils.GenerateBlocksetID());
            var blockset = _makeBlocksetFromModel(newModel);
            return blockset;
        }

        private Block_2 _makeBlockFromModel(BlockModel model)
        {
            var block = BlockFactory.MakeBlock(model, _makeNewBlockset);
            block.SwitchSection?.GenerateDefaultSections();
            block.ScaleProvider = _blocksetScaleProvider;

            Blocks[model] = block;
            AddChildAfterUpdate(block);
            return block;
        }

        private Blockset_2 _makeBlocksetFromModel(BlocksetModel model)
        {
            var blockset = BlockFactory.MakeBlockset(model);

            blockset.ScaleProvider = _blocksetScaleProvider;
            blockset.BlockLiftedFromThisCallback = MakeNewBlocksetWithLiftedBlocks;
            blockset.ThisDroppedCallback = BlocksetDropped;

            Blocksets[model] = blockset;
            AddChildAfterUpdate(blockset);
            return blockset;
        }



        private void _disposeBlockset(Blockset_2 blockset)
        {
            Model.DeleteBlockset(blockset.Model);
            RemoveChild(blockset); 

            Blocksets.Remove(blockset.Model);

            var blocks = blockset.DetachAllBlocks();
            _disposeBlocks(blocks);
            blockset.Dispose();
        }

        private void _disposeBlocks(List<Block_2> blocks)
        {
            Model.DeleteBlocks(blocks);
            RemoveChildren(blocks);

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

        private MenuItemScaleProviderParent _blocksetScaleProvider;
        private float _chipScale { get => _blocksetScaleProvider.Multiplier; set => _blocksetScaleProvider.SetManualScale(value); }

        private void _changeChipScale(float multiplier)
        {
            var newMultipier = _blocksetScaleProvider.GetScale(this) * multiplier;
            if (newMultipier > 1.995)
            {
                _chipScale *= multiplier;
            }
        }

        private void _correctScale()
        {
            var currentChipScale = _blocksetScaleProvider.GetScale(this);
            if (currentChipScale < 2)
            {
                _chipScale = 2.0f / MenuScreen.Scale;
            }
        }

        private Blockset_2 _getBlocksetMouseIsOver(Blockset_2 held) => Blocksets.Values.Where(b => b.CanDropThisOn(held)).OrderBy(c => c.DrawLayer).FirstOrDefault();

        private void _cleanUpDisposedBlocksets()
        {
            var toDispose = Blocksets.Values.Where(b => b.ShouldDispose).ToList();
            toDispose.ForEach(b => _disposeBlockset(b));
        }
    }
}
