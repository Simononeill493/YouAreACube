using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlockValues = System.Tuple<IAmACube.BlockModel, IAmACube.ChipInputValues>;

namespace IAmACube
{
    static class Parser_ChipsetsToBlockModel
    {
        public static FullModel ToBlockModel(this ChipsetCollection chipsetCollection, TemplateVariableSet variables)
        {
            var fullModel = new FullModel();
            var inputValues = new List<BlockValues>();

            foreach (var subChipset in chipsetCollection.GetAllChipsets())
            {
                var blocksetModel = fullModel.CreateBlockset(subChipset.Name,isInternal: !chipsetCollection.HasMode(subChipset));
                foreach (var chip in subChipset.Chips)
                {
                    var blockModel = fullModel.CreateBlock(chip.Name, chip.GetBlockData().GetThisOrBaseMappingBlock());
                    fullModel.AddInputs(blockModel);
                    blocksetModel.Blocks.Add(blockModel);

                    var inputPinValues = chip.GetInputPinValues();
                    inputValues.Add(new BlockValues(blockModel, inputPinValues));
                }
            }

            fullModel.Initial = fullModel.Blocksets[chipsetCollection.Initial.Name];

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

            foreach (var controlChip in chipsetCollection.GetAllControlChips())
            {
                var controlChipModel = fullModel.Blocks[((IChip)controlChip).Name];
                foreach (var subChipset in controlChip.GetSubChipsets())
                {
                    var referencedChipset = fullModel.Blocksets[subChipset.Item2.Name];
                    controlChipModel.SubBlocksets.Add((subChipset.Item1, referencedChipset));
                }
            }

            foreach(var blockModel in fullModel.Blocks.Values)
            {
                InputModelSpecialCases(blockModel, fullModel.Blocksets.Values);
            }

            fullModel.Sort();
            return fullModel;
        }

        public static void Sort(this FullModel fullModel)
        {
            fullModel.Blocks = fullModel.Blocks.Values.OrderBy(b => b.Name).ToDictionary(b => b.Name);
            fullModel.Blocksets = fullModel.Blocksets.Values.OrderBy(b => b.ModeIndex).ThenBy(b=>b.Name).ToDictionary(b => b.Name);
        }

        public static void InputModelSpecialCases(BlockModel model,IEnumerable<BlocksetModel> blocks)
        {
            var data = model.GetVisualBlockData();

            for(int i =0;i<data.NumInputs;i++)
            {
                var blockInputModel = model.Inputs[i];

                switch (data.SpecialInputTypes[i])
                {
                    case BlockSpecialInputType.None:
                        continue;
                    case BlockSpecialInputType.MetaVariable:
                        var variableOption = BlockInputOption.CreateMetaVariable((int)blockInputModel.InputOption.Value);
                        blockInputModel.InputOption = variableOption;
                        break;
                    case BlockSpecialInputType.Chipset:
                        var matchingBlocksetModels = blocks.Where(b => b.ModeIndex == ((int)blockInputModel.InputOption.Value));
                        if(matchingBlocksetModels.Count()!=1) 
                        {
                            throw new Exception();
                        }

                        var chipsetOption = BlockInputOption.CreateChipset(matchingBlocksetModels.First());
                        blockInputModel.InputOption = chipsetOption;
                        break;
                    default:
                        throw new NotImplementedException();
                }

            }
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
