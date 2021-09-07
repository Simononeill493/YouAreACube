using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class Parser_ChipsetToBlockModel
    {
        public static FullModel ToBlockModel(this Chipset chipset,TemplateVariableSet variables)
        {
            var modelFull= new FullModel();
            var inputValues = new Dictionary<BlockModel, List<(InputOptionType, object)>>();

            foreach(var subChipset in chipset.GetChipsetAndSubChipsets())
            {
                var blocksetModel = modelFull.CreateBlockset(subChipset.Name);
                foreach(var chip in subChipset.Chips)
                {
                    var blockModel = modelFull.CreateBlock(chip.Name,chip.GetBlockData());
                    blocksetModel.Blocks.Add(blockModel);

                    inputValues[blockModel] = chip.GetInputPinValues(chip.GetBlockData());                    
                }
            }

            foreach(var model_input in inputValues)
            {
                var model = model_input.Key;
                foreach(var inputValue in model_input.Value)
                {
                    BlockInputOption_2 inputOption = null;

                    switch (inputValue.Item1)
                    {
                        case InputOptionType.Value:
                            inputOption = BlockInputOption_2.CreateValue(inputValue.Item2);
                            break;
                        case InputOptionType.Reference:
                            var blockToReference = modelFull.Blocks[((IChip)inputValue.Item2).Name];
                            inputOption = BlockInputOption_2.CreateReference(blockToReference);
                            break;
                        case InputOptionType.Variable:
                            inputOption = BlockInputOption_2.CreateVariable(variables.Dict[(int)inputValue.Item2]);
                            break;
                        default:
                            throw new Exception();
                    }

                    var inputModel = new BlockInputModel() { InputOption = inputOption };

                    model.Inputs.Add(inputModel);
                    modelFull.InputParents[inputModel] = model;
                }
            }

            foreach(var controlChip in chipset.ControlChips)
            {
                var controlChipModel = modelFull.Blocks[((IChip)controlChip).Name];
                foreach(var subChipset in controlChip.GetBaseSubChipsets())
                {
                    var referencedChipset = modelFull.Blocksets[subChipset.Item2.Name];
                    controlChipModel.SubBlocksets.Add((subChipset.Item1, referencedChipset));
                }
            }

            return modelFull;
        }
    }
}
