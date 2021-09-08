using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockFactory
    {
        public static Blockset MakeBlockset(BlocksetModel model)
        {
            var blockset = new Blockset(model);
            return blockset;
        }

        public static Block MakeBlock(BlockModel model)
        {
            var block = new Block(model);
            var data = model.GetBlockData();

            AddBlockTop(block, data);
            AddInputSections(block, data);
            AddSwitchSections(block, data);

            ConfigureBlockAppearance(block,data);

            return block;
        }

        public static void AddBlockTop(Block block, BlockData data)
        {
            var blockTop = new BlockTop(block, data.Name);
            block.Top = blockTop;

            _addSection(block, blockTop);
        }

        public static void AddInputSections(Block block, BlockData data)
        {
            var inputs = block.Model.Inputs;

            for(int i=0;i<data.NumInputs;i++)
            {
                var inputSection = MakeInputSection(GetDrawLayerForNewSection(block), data,inputs[i],i);
                _addSection(block, inputSection);
            }
        }

        public static void AddSwitchSections(Block block, BlockData data)
        {
            if (BlockDataUtils.IsSwitchBlock(data))
            {
                var switchSection = MakeSwitchSection(data, block.Model,GetDrawLayerForNewSection(block));
                _addSection(block, switchSection);
                block.SwitchSection = switchSection;
            }
        }

        public static BlockInputSection MakeInputSection(IHasDrawLayer layer, BlockData data, BlockInputModel model, int inputIndex)
        {
            var name = data.GetInputDisplayName(inputIndex);
            var inputSection = new BlockInputSection(layer, name);

            var dropdown = MakeDropdown(inputSection, data.GetInputTypes(inputIndex), model);
            dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentageOffset, true);

            inputSection.AddChild(dropdown);
            return inputSection;
        }

        public static BlockInputDropdown MakeDropdown(BlockInputSection inputSection,List<string> inputTypes, BlockInputModel model)
        {
            var dropdown = new BlockInputDropdown(inputSection,inputTypes,model,()=>model.DisplayValue);
            return dropdown;
        }

        public static BlockSwitchSection_2 MakeSwitchSection(BlockData data, BlockModel model,IHasDrawLayer layer)
        {
            var switchSection = new BlockSwitchSection_2(layer,model,data.GetDefaultSwitchSections());
            return switchSection;
        }

        public static IHasDrawLayer GetDrawLayerForNewSection(Block block) =>  ManualDrawLayer.InFrontOf(block, block.Sections.Count+1);

        private static void _addSection(Block block, SpriteMenuItem section)
        {
            var currentSize = block.GetBaseSize();

            section.SetLocationConfig(0, currentSize.Y, CoordinateMode.ParentPixelOffset, false);
            block.AddChild(section);
            block.Sections.Add(section);
        }

        private static void ConfigureBlockAppearance(Block block, BlockData data)
        {
            block.Sections.ForEach(s => s.ColorMask = data.GetColor());
            _setEndSection(block);
        }

        private static void _setEndSection(Block block)
        {
            if(block.Sections.Count>1)
            {
                block.Sections.Last().SpriteName = BuiltInMenuSprites.BlockBottom;
            }
        }
    }
}
