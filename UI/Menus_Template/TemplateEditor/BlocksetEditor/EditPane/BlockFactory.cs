using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DropdownGenerator = System.Func<IAmACube.BlockInputSection, int, IAmACube.BlockInputDropdown>;

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
            var data = model.GetVisualBlockData();

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
            if (data.Name.Equals("SetVariable"))
            {
                AddSetVariableInputSections(block, data);
            }
            else if (data.Name.Equals("IsVariableSet"))
            {
                AddCheckVariableSetInputSections(block, data);
            }
            else
            {
                AddDefaultInputSections(block, data);
            }
        }

        public static void AddDefaultInputSections(Block block, BlockData data)
        {
            for (int index = 0; index < data.NumInputs; index++)
            {
                AddDefaultInputSection(block, data, index);
            }
        }

        public static void AddDefaultInputSection(Block block, BlockData data,int index)
        {
            var inputSection = MakeInputSection(block, data, index);
            var dropdown = MakeDefaultDropdown(block, inputSection, data.GetInputTypes(index), block.Model.Inputs[index]);
            inputSection.AddChild(dropdown);
            _addSection(block, inputSection);
        }

        public static void AddSetVariableInputSections(Block block, BlockData data)
        {
            var inputSection = MakeInputSection(block, data, 0);
            var inputSection2 = MakeInputSection(block, data, 1);

            var dropdown = MakeMetaVariableDropdown(block, inputSection, block.Model.Inputs[0]);
            var dropdown2 = MakeDefaultDropdown(block, inputSection2, data.GetInputTypes(1), block.Model.Inputs[1]);

            inputSection.AddChild(dropdown);
            inputSection2.AddChild(dropdown2);

            _addSection(block, inputSection);
            _addSection(block, inputSection2);

            dropdown2.SetInputTypeProvider(dropdown.GetTypesOfSelectedVariable);
        }

        public static void AddCheckVariableSetInputSections(Block block, BlockData data)
        {
            var inputSection = MakeInputSection(block, data, 0);
            var dropdown = MakeMetaVariableDropdown(block, inputSection, block.Model.Inputs[0]);
            inputSection.AddChild(dropdown);
            _addSection(block, inputSection);
        }

        public static BlockInputSection MakeInputSection(Block block, BlockData data, int index) => new BlockInputSection(GetDrawLayerForNewSection(block), data.GetInputDisplayName(index));

        public static BlockInputDropdown MakeDefaultDropdown(Block block,BlockInputSection inputSection, List<string> inputTypes, BlockInputModel model)
        {
            var dropdown = new BlockInputDropdown(inputSection, inputTypes, model, () => model.DisplayValue);
            dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentageOffset, true);
            dropdown.OnSelectedChanged += (o) => block.DropdownItemSelected(o, dropdown.Model);
            return dropdown;
        }

        public static BlockInputDropdownMetaVariable MakeMetaVariableDropdown(Block block,BlockInputSection inputSection, BlockInputModel model)
        {
            var dropdown = new BlockInputDropdownMetaVariable(inputSection, model, () => model.DisplayValue);
            dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentageOffset, true);
            dropdown.OnSelectedChanged += (o) => block.DropdownItemSelected(o, dropdown.Model);

            return dropdown;
        }


        public static void AddSwitchSections(Block block, BlockData data)
        {
            if (BlockDataUtils.IsSwitchBlock(data))
            {
                var switchSection = MakeSwitchSection(data, block.Model,GetDrawLayerForNewSection(block));
                block.SwitchSection = switchSection;
                _addSection(block, switchSection);
            }
        }



        public static BlockSwitchSection MakeSwitchSection(BlockData data, BlockModel model,IHasDrawLayer layer)
        {
            var switchSection = new BlockSwitchSection(layer,model,data.GetDefaultSwitchSections());
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
