using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlockValues = System.Tuple<IAmACube.BlockModel, IAmACube.ChipInputValues>;

namespace IAmACube
{
    static class Parser_ChipsetToBlockModel
    {
        public static FullModel ToBlockModel(this Chipset chipset, TemplateVariableSet variables)
        {
            var fullModel = new FullModel();
            var inputValues = new List<BlockValues>();

            foreach (var subChipset in chipset.GetThisAndAllChipsetsCascade())
            {
                var blocksetModel = fullModel.CreateBlockset(subChipset.Name);
                foreach (var chip in subChipset.Chips)
                {
                    var blockModel = fullModel.CreateBlock(chip.Name, chip.GetBlockData());
                    fullModel.AddInputs(blockModel);
                    blocksetModel.Blocks.Add(blockModel);

                    var inputPinValues = chip.GetInputPinValues();
                    inputValues.Add(new BlockValues(blockModel, inputPinValues));
                }
            }

            foreach (var blockAndInputValues in inputValues)
            {
                var blockModel = blockAndInputValues.Item1;
                foreach (var inputValue in blockAndInputValues.Item2.List)
                {
                    var inputModel = CreateInputModel(inputValue, fullModel, variables);

                    blockModel.Inputs.Add(inputModel);
                    fullModel.InputParents[inputModel] = blockModel;
                }
            }

            foreach (var controlChip in chipset.GetControlChipsCascade())
            {
                var controlChipModel = fullModel.Blocks[((IChip)controlChip).Name];
                foreach (var subChipset in controlChip.GetSubChipsets())
                {
                    var referencedChipset = fullModel.Blocksets[subChipset.Item2.Name];
                    controlChipModel.SubBlocksets.Add((subChipset.Item1, referencedChipset));
                }
            }

            return fullModel;
        }

        public static BlockInputModel CreateInputModel(ChipInputValue option, FullModel fullModel, TemplateVariableSet variables)
        {
            BlockInputOption inputOption;
            switch (option.OptionType)
            {
                case InputOptionType.Value:
                    inputOption = BlockInputOption.CreateValue(option.OptionValue);
                    break;
                case InputOptionType.Reference:
                    var blockToReference = fullModel.Blocks[((IChip)option.OptionValue).Name];
                    inputOption = BlockInputOption.CreateReference(blockToReference);
                    break;
                case InputOptionType.Variable:
                    inputOption = BlockInputOption.CreateVariable(variables.Dict[(int)option.OptionValue]);
                    break;
                default:
                    throw new Exception();
            }

            return new BlockInputModel() { InputOption = inputOption };
        }
    }


}
