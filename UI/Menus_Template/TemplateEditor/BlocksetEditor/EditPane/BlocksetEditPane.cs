﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class BlocksetEditPane : SpriteMenuItem
    {
        public static Dictionary<BlocksetModel, Blockset> Blocksets;
        public static Dictionary<BlockModel, Block> Blocks;

        public static FullModel Model;
        public static IVariableProvider VariableProvider;

        public Action<BlockInputOption, BlockInputModel> OpenSubMenuCallback;

        private MenuItem _bin;

        public BlocksetEditPane(IVariableProvider variableProvider,MenuItem bin) : base(ManualDrawLayer.Create(DrawLayers.BackgroundLayer),BuiltInMenuSprites.BlocksetEditPane)
        {
            VariableProvider = variableProvider;
            _bin = bin;

            Blocksets = new Dictionary<BlocksetModel, Blockset>();
            Blocks = new Dictionary<BlockModel, Block>();

            var plusButton = new SpriteMenuItem(ManualDrawLayer.Create(DrawLayers.MenuBaseLayer), BuiltInMenuSprites.PlusButton);
            plusButton.SetLocationConfig(GetBaseSize().X - 9, 0, CoordinateMode.ParentPixel, false);
            plusButton.OnMouseReleased += (i) => _changeChipScale(2);
            AddChild(plusButton);

            var minusButton = new SpriteMenuItem(ManualDrawLayer.Create(DrawLayers.MenuBaseLayer), BuiltInMenuSprites.MinusButton_Partial);
            minusButton.SetLocationConfig(GetBaseSize().X - 17, 0, CoordinateMode.ParentPixel, false);
            minusButton.OnMouseReleased += (i) => _changeChipScale(0.5f);
            AddChild(minusButton);

            _blockScaler = new MenuItemScaleProviderParent();
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            _cleanUpDisposedBlocksets();
            _checkForPan(input);
        }

        public void LoadChipsetForEditing(CubeTemplate template)
        {
            if(!template.Active) { return; }
            var model = template.Chipsets.Initial.ToBlockModel(VariableProvider.GetVariables());
            var initial = _loadModel(model);
            initial.SetLocationConfig(10, 10, CoordinateMode.ParentPixel);
        }

        public void AddEditedChipsetToTemplate(CubeTemplate template)
        {
            _setInitial();

            TemplateParsingTester.TestParsingRoundTrip(template.Name, Model, VariableProvider.GetVariables());
            template.Chipsets = new ChipsetCollection(Model.ToChipset());
        }


        public void RecieveFromSearchPane(BlockData data, UserInput input)
        {
            var block = _makeNewBlock(data);

            var blockset = _makeNewBlockset(isInternal: false);
            blockset.SetInitialDragState(this, input);
            blockset.AddBlock(block, 0);
        }

        public void BlocksLifted(Blockset blockset,List<Block> blocks, UserInput input)
        {
            blocks.First().ManuallyEndDrag(input);

            var heldBlockset = (blockset.Empty) ? blockset : _makeNewBlockset(isInternal: false);
            heldBlockset.SetInitialDragState(this, input);
            heldBlockset.AddBlocks(blocks, 0);
        }

        public void BlocksetDropped(Blockset blockset, UserInput input)
        {
            if(_bin.MouseHovering)
            {
                blockset.ManuallyDispose();
                return;
            }

            var droppable = Blocksets.Values.Where(b => b.CanDropThisOn(blockset));
            var hoveringOver = droppable.OrderByDescending(c => Blocksets.GetDepth(c)).FirstOrDefault();
            if(hoveringOver!=null)
            {
                AppendBlockset(blockset, hoveringOver);
                return;
            }

            _alignToPixelGrid(blockset);
        }

        public void AppendBlockset(Blockset toAppend, Blockset target) => target.DropBlocks(toAppend.DetachAllBlocks());








        private Block _makeNewBlock(BlockData data)
        {
            var newModel = Model.CreateBlock(IDUtils.GenerateBlockID(data), data);
            newModel.MakeBlankInputs();
            Model.AddInputs(newModel);

            var newBlock = _makeBlockFromModel(newModel);
            newBlock.SwitchSection?.GenerateDefaultSections();

            return newBlock;
        }

        private Blockset _makeNewBlockset(bool isInternal)
        {
            var newModel = Model.CreateBlockset(IDUtils.GenerateBlocksetID(),isInternal);
            var newBlockset = _makeBlocksetFromModel(newModel);

            return newBlockset;
        }

        private Blockset _loadModel(FullModel model)
        {
            Model = model;

            foreach (var blocksetModel in model.Blocksets.Values)
            {
                var blockset = _makeBlocksetFromModel(blocksetModel);
            }

            foreach (var blocksetModel in model.Blocks.Values)
            {
                _makeBlockFromModel(blocksetModel);
            }

            foreach (var block in Blocks.Values)
            {
                block.SwitchSection?.InitializeAllSections();
            }

            var externals = Blocksets.Values.Where(b => !b.IsInternal);
            if (externals.Count()!=1)
            {
                throw new Exception();
            }

            var initial = Blocksets.Values.Where(b => b.Model.Name == ChipsetCollection.InitialChipName).FirstOrDefault();
            if(initial == null)
            {
                throw new Exception();
            }
            return initial;
        }

        private Block _makeBlockFromModel(BlockModel model)
        {
            var block = BlockFactory.MakeBlock(model);
            block.SwitchSection?.SetGenerator(()=>_makeNewBlockset(isInternal: true));
            block.ScaleProvider = _blockScaler;

            Blocks[model] = block;
            AddChildAfterUpdate(block);
            return block;
        }


        private Blockset _makeBlocksetFromModel(BlocksetModel model)
        {
            var blockset = BlockFactory.MakeBlockset(model);

            blockset.ScaleProvider = _blockScaler;
            blockset.BlocksLiftedFromThisCallback = BlocksLifted;
            blockset.ThisDroppedCallback = BlocksetDropped;
            blockset.OpenSubMenuCallback = OpenSubMenuCallback;

            Blocksets[model] = blockset;
            AddChildAfterUpdate(blockset);
            return blockset;
        }

        private void _disposeBlockset(Blockset blockset)
        {
            Model.DeleteBlockset(blockset.Model);
            Blocksets.Remove(blockset.Model);

            _disposeBlocks(blockset.DetachAllBlocks());

            RemoveChild(blockset);
            blockset.Dispose();
        }

        private void _disposeBlocks(List<Block> blocks)
        {
            Model.DeleteBlocks(blocks);
            
            foreach (var block in blocks)
            {
                Blocks.Remove(block.Model);

                block.GetSubBlocksets().ForEach(s => _disposeBlockset(s));

                RemoveChild(block);
                block.Dispose();
            }
        }

        private void _cleanUpDisposedBlocksets()
        {
            var toDispose = Blocksets.Values.Where(b => b.ShouldDispose).ToList();
            toDispose.ForEach(b => _disposeBlockset(b));
        }

        private void _alignToPixelGrid(Blockset blockset)
        {
            var totalDist = blockset.ActualLocation - ActualLocation;
            var trueScale = (int)(Scale * _blockScale);
            var pixelDist = (totalDist / trueScale);
            blockset.SetLocationConfig(pixelDist, CoordinateMode.ParentPixel, false);
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            _correctScale();
            this.SetPositions();
            _updateChildLocations();

            base._drawSelf(drawingInterface);
        }
        private void _correctScale()
        {
            var currentChipScale = _blockScaler.GetScale(this);
            if (currentChipScale < 2)
            {
                _blockScale = 2.0f / MenuScreen.Scale;
            }
        }

        private MenuItemScaleProviderParent _blockScaler;
        private float _blockScale { get => _blockScaler.Multiplier; set => _blockScaler.SetManualScale(value); }

        private void _changeChipScale(float multiplier)
        {
            var newMultipier = _blockScaler.GetScale(this) * multiplier;
            if (newMultipier > 1.995)
            {
                _blockScale *= multiplier;
            }
        }


        private void _setInitial()
        {
            var topLevelBlocksets = Blocksets.Values.Where(b => !b.IsInternal);
            if (topLevelBlocksets.Count() != 1)
            {
                throw new Exception("Multiple top level blocksets");
            }
            topLevelBlocksets.First().Model.Name = ChipsetCollection.InitialChipName;
        }
    }
}
