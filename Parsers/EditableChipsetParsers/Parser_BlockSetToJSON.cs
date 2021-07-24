using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class Parser_BlockSetToJSON
    {
        public static string ParseEditableChipsetToJson(Blockset chipset)
        {
            var chipsetsJson = new FullChipsetJSONData();

            foreach (var editableChipset in chipset.GetThisAndSubChipsets())
            {
                var chipsetJson = new ChipsetJSONData(editableChipset);
                chipsetsJson.Add(chipsetJson);

                foreach(var chip in editableChipset.Blocks)
                {
                    var chipJObject = _parseChipTop(chip);
                    chipsetJson.Chips.Add(chipJObject);
                }
            }

            chipsetsJson.AlphabetSort();

            var jobjectList = JToken.FromObject(chipsetsJson, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
            return jobjectList.ToString();
        }

        private static ChipJSONData _parseChipTop(BlockTop chip)
        {
            var chipJObject = new ChipJSONData(chip);

            var mappedChipType = _getChipSubMapping(chipJObject);
            _setChipTypeArguments(chipJObject, mappedChipType);
            _parseSelectedChipInputs(chipJObject);
            _setControlChipTargets(chipJObject);

            return chipJObject;
        }

        public static BlockData _getChipSubMapping(ChipJSONData chipJObject)
        {
            if (chipJObject.GraphicalChipData.IsMappedToSubChips)
            {
                var selectedTypes = chipJObject.Block.GetSelectedInputTypes();
                var mappedChipType = _getFirstMatchingMapping(selectedTypes, chipJObject.GraphicalChipData.InputMappings);

                chipJObject.ActualChipType = mappedChipType.Name;
                return mappedChipType;
            }

            return null;
        }

        public static void _setChipTypeArguments(ChipJSONData chipJObject, BlockData mappedChipType)
        {
            var chip = chipJObject.Block;

            if (chip.CurrentTypeArguments.Count > 0 & chip.CurrentTypeArguments.First().Length > 0 & mappedChipType == null)
            {
                chipJObject.TypeArguments = chip.CurrentTypeArguments;
            }

            if (mappedChipType != null)
            {
                if (mappedChipType.IsGeneric)
                {
                    chipJObject.TypeArguments = chip.CurrentTypeArguments;
                }
            }
        }

        public static void _parseSelectedChipInputs(ChipJSONData chipJObject)
        {
            chipJObject.Inputs = new List<ChipJSONInputData>();
            var inputsList = chipJObject.Block.GetCurrentInputs();
            for (int i = 0; i < inputsList.Count; i++)
            {
                var input = inputsList[i];
                var inputOptionType = input.OptionType.ToString();
                if (inputOptionType.Equals(nameof(InputOptionType.Parseable)))
                {
                    inputOptionType = nameof(InputOptionType.Value);
                }

                var inputData = new ChipJSONInputData(inputOptionType, input.ToString());
                chipJObject.Inputs.Add(inputData);
            }
        }

        private static void _setControlChipTargets(ChipJSONData chipJObject)
        {
            if(chipJObject.GraphicalChipData.Name.Equals("If"))
            {
                var ifChip = (BlockTopSwitch)chipJObject.Block;

                chipJObject.Yes = ifChip.SwitchBlocksets[0].Name;
                chipJObject.No = ifChip.SwitchBlocksets[1].Name;

            }
            if (chipJObject.GraphicalChipData.Name.Equals("KeySwitch"))
            {
                var keyChip = (BlockTopSwitch)chipJObject.Block;

                chipJObject.KeyEffects = new List<(string, string)>();

                foreach (var keyAndChipset in keyChip.GetSwitchSectionsWithNames())
                {
                    chipJObject.KeyEffects.Add((keyAndChipset.Item1, keyAndChipset.Item2.Name));
                }
            }
        }

        public static BlockData _getFirstMatchingMapping(List<string> inputs, List<BlockData> possibleMatches)
        {
            foreach (var possibleMatch in possibleMatches)
            {
                if(_doesMappingMatchInputs(inputs,possibleMatch))
                {
                    return possibleMatch;
                }
            }

            return null;
        }
        public static bool _doesMappingMatchInputs(List<string> inputs,BlockData possibleMatch)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                if (!TemplateEditUtils.IsValidInputFor(inputs[i], possibleMatch.Inputs[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
