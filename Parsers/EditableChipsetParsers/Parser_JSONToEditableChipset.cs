using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class Parser_JSONToEditableChipset
    {
        public static Blockset ParseJsonToBlockset(string json, IBlocksetGenerator generator)
        {
            var blocksetsJson = JsonConvert.DeserializeObject<FullChipsetJSONData>(json);
            var blocksDict = blocksetsJson.GetChipsetsDict();
            var chipsDict = blocksetsJson.GetChipsDict();

            blocksetsJson.SetChipData();
            blocksetsJson.CreateEditableChipsetObjects(generator);

            _appendChipsToChipsets(blocksetsJson);

            foreach (var chip in chipsDict.Values)
            {
                _setValuesForInputSections(chip, chipsDict);
                _setControlChipTargets(chip, blocksDict);
            }

            var baseBlockset = blocksetsJson.GetInitial().Blockset;
            baseBlockset.AddAndRemoveQueuedChildren_Cascade();

            return baseBlockset;
        }

        private static void _appendChipsToChipsets(FullChipsetJSONData chipsetsJson)
        {
            foreach (var blockJson in chipsetsJson)
            {
                foreach (var chipJson in blockJson.Chips)
                {
                    blockJson.Blockset.AppendBlockToBottom(chipJson.Block);
                }
            }
        }
        private static void _setControlChipTargets(ChipJSONData chip, Dictionary<string, ChipsetJSONData> chipsets)
        {
            if (chip.BlockData.Name.Equals("If"))
            {
                var ifChip = (BlockTopSwitch)chip.Block;

                ifChip.AddSwitchSection("Yes", chipsets[chip.Yes].Blockset);
                ifChip.AddSwitchSection("No", chipsets[chip.No].Blockset);
            }
            if (chip.BlockData.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (BlockTopSwitch)chip.Block;

                foreach (var (keyString, blockName) in chip.KeyEffects)
                {
                    var block = chipsets[blockName].Blockset;
                    keySwitchChip.AddSwitchSection(keyString, block);
                }
            }
        }
        
        private static void _setValuesForInputSections(ChipJSONData chip, Dictionary<string, ChipJSONData> chipsDict)
        {
            for (int i = 0; i < chip.Inputs.Count; i++)
            {
                _setValuesForInputSection(chip, chipsDict, i);
            }
        }
        private static void _setValuesForInputSection(ChipJSONData chip,Dictionary<string,ChipJSONData> chipsDict,int inputIndex)
        {
            var jsonInputData = chip.Inputs[inputIndex];
            var inputOption = _parseInputOption(chip,chipsDict,jsonInputData.InputType,jsonInputData.InputValue,inputIndex);

            chip.Block.ManuallySetInputSection(inputOption, inputIndex);
        }

        private static BlockInputOption _parseInputOption(ChipJSONData chip, Dictionary<string, ChipJSONData> chipsDict, string inputType, string inputValue, int inputIndex)
        {
            if (inputType.Equals("Reference"))
            {
                return _parseReferenceChipInput(chipsDict, inputValue);
            }
            else if (inputType.Equals("Value"))
            {
                return _parseValueChipInput(chip, inputIndex);
            }

            throw new Exception("Unrecognized chip input option type");
        }
        private static BlockInputOption _parseReferenceChipInput(Dictionary<string, ChipJSONData> chipsDict, string inputValue)
        {
            var referenceChip = (BlockTopWithOutput)chipsDict[inputValue].Block;
            return new BlockInputOptionReference(referenceChip);
        }
        private static BlockInputOption _parseValueChipInput(ChipJSONData chip, int inputIndex)
        {
            var value = chip.ParseInput(inputIndex);
            return new BlockInputOptionValue(value);
        }
    }
}
