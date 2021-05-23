using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class JSONToEditableChipsetParser
    {
        public static EditableChipset ParseJsonToEditableChipset(string json, IChipsetGenerator generator)
        {
            var chipsetsJson = JsonConvert.DeserializeObject<ChipsetJSONData>(json);
            var blocksDict = chipsetsJson.GetBlocksDict();
            var chipsDict = chipsetsJson.GetChipsDict();

            chipsetsJson.SetChipData();
            chipsetsJson.CreateEditableChipsetObjects(generator);

            _appendChipsToChipsets(chipsetsJson);

            foreach (var chip in chipsDict.Values)
            {
                _setValuesForInputSections(chip, chipsDict);
                _setControlChipTargets(chip, blocksDict);
            }

            var baseChipset = chipsetsJson.GetInitial().Chipset;
            baseChipset.AddAndRemoveQueuedChildren_Cascade();

            return baseChipset;
        }

        private static void _appendChipsToChipsets(ChipsetJSONData chipsetsJson)
        {
            foreach (var blockJson in chipsetsJson)
            {
                foreach (var chipJson in blockJson.Chips)
                {
                    blockJson.Chipset.AppendToBottom(chipJson.ChipTop);
                }
            }
        }
        private static void _setControlChipTargets(ChipJSONData chip, Dictionary<string, ChipBlockJSONData> chipsets)
        {
            if (chip.ChipData.Name.Equals("If"))
            {
                var ifChip = (ChipTopSwitch)chip.ChipTop;

                ifChip.AddSwitchSection("Yes", chipsets[chip.Yes].Chipset);
                ifChip.AddSwitchSection("No", chipsets[chip.No].Chipset);
            }
            if (chip.ChipData.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (ChipTopSwitch)chip.ChipTop;

                foreach (var (keyString, blockName) in chip.KeyEffects)
                {
                    var block = chipsets[blockName].Chipset;
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

            chip.ChipTop.ManuallySetInputSection(inputOption, inputIndex);
        }

        private static ChipInputOption _parseInputOption(ChipJSONData chip, Dictionary<string, ChipJSONData> chipsDict, string inputType, string inputValue, int inputIndex)
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
        private static ChipInputOption _parseReferenceChipInput(Dictionary<string, ChipJSONData> chipsDict, string inputValue)
        {
            var referenceChip = (ChipTopWithOutput)chipsDict[inputValue].ChipTop;
            return new ChipInputOptionReference(referenceChip);
        }
        private static ChipInputOption _parseValueChipInput(ChipJSONData chip, int inputIndex)
        {
            var value = chip.ParseInput(inputIndex);
            return new ChipInputOptionValue(value);
        }
    }
}
