using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SectionAndDropdown = System.Tuple<IAmACube.BlockInputSection, IAmACube.BlockInputDropdown>;

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

            var blockTop = AddBlockTop(block, data);
            var sections = AddInputSections(block, data);
            var switchSection = AddSwitchSection(block, data);

            ConfigureSpecificBlocks(block, data, sections);
            ConfigureBlockAppearance(block,data);

            return block;
        }

        public static BlockTop AddBlockTop(Block block, BlockData data)
        {
            var blockTop = new BlockTop(block, data.Name);
            block.Top = blockTop;

            _addSection(block, blockTop);
            return blockTop;
        }

        public static List<SectionAndDropdown> AddInputSections(Block block, BlockData data)
        {
            var sections = new List<SectionAndDropdown>();
            for (int index = 0; index < data.NumInputs; index++)
            {
                var section = AddInputSection(block, data, index);
                sections.Add(section);
            }

            return sections;
        }
        public static SectionAndDropdown AddInputSection(Block block, BlockData data, int index)
        {
            var inputSection = MakeInputSection(block, data, index);
            var dropdown = MakeDropdown(block,inputSection,block.Model.Inputs[index],data.GetInputTypes(index),data.SpecialInputTypes[index]);
            inputSection.AddChild(dropdown);
            _addSection(block, inputSection);

            return new SectionAndDropdown(inputSection, dropdown);
        }
        
        public static BlockInputSection MakeInputSection(Block block, BlockData data, int index) => new BlockInputSection(_getDrawLayerForNewSection(block), data.GetInputDisplayName(index));
        public static BlockInputDropdown MakeDropdown(Block block,BlockInputSection inputSection,BlockInputModel model,List<string> inputTypes,BlockSpecialInputType specialInputType)
        {
            BlockInputDropdown dropdown = null;
            switch (specialInputType)
            {
                case BlockSpecialInputType.None:
                    dropdown = new BlockInputDropdown(inputSection, inputTypes, model, () => model.DisplayValue);
                    break;
                case BlockSpecialInputType.MetaVariable:
                    dropdown = new BlockInputDropdownMetaVariable(inputSection, model, () => model.DisplayValue);
                    break;
                case BlockSpecialInputType.Chipset:
                    dropdown = new BlockInputDropdownChipsetSelector(inputSection, model, () => model.DisplayValue);
                    break;
                default:
                    throw new NotImplementedException();
            }

            dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentage, true);
            dropdown.OnSelectedChanged += (o) => block.DropdownItemSelected(o, dropdown.Model);
            return dropdown;
        }

        public static BlockSwitchSection AddSwitchSection(Block block, BlockData data)
        {
            if (BlockDataUtils.IsSwitchBlock(data))
            {
                var switchSection = MakeSwitchSection(data, block.Model, _getDrawLayerForNewSection(block));
                block.SwitchSection = switchSection;
                _addSection(block, switchSection);

                return switchSection;
            }

            return null;
        }
        public static BlockSwitchSection MakeSwitchSection(BlockData data, BlockModel model, IHasDrawLayer layer)
        {
            var switchSection = new BlockSwitchSection(layer, model, data.GetDefaultSwitchSections());
            return switchSection;
        }

        public static void ConfigureSpecificBlocks(Block block, BlockData data, List<SectionAndDropdown> sections)
        {
            //TODO this is kind of awkward, go back to it later
            if (data.Name.Equals("SetVariable"))
            {
                ConfigureSetVariableBlock(block,sections);
            }
        }
        public static void ConfigureSetVariableBlock(Block block, List<SectionAndDropdown> sections)
        {
            var dropdown1 = (BlockInputDropdownMetaVariable)sections[0].Item2;
            var dropdown2  = sections[1].Item2;

            dropdown2.SetInputTypeProvider(dropdown1.GetTypesOfSelectedVariable);
        }

        public static void ConfigureBlockAppearance(Block block, BlockData data)
        {
            block.Sections.ForEach(s => s.ColorMask = data.GetColor());

            if (block.Sections.Count > 1)
            {
                block.Sections.Last().SpriteName = BuiltInMenuSprites.BlockBottom;
            }
        }






        private static IHasDrawLayer _getDrawLayerForNewSection(Block block) =>  ManualDrawLayer.InFrontOf(block, block.Sections.Count+1);

        private static void _addSection(Block block, SpriteMenuItem section)
        {
            var currentSize = block.GetBaseSize();

            section.SetLocationConfig(0, currentSize.Y, CoordinateMode.ParentPixel, false);
            block.AddChild(section);
            block.Sections.Add(section);
        }
    }
}
