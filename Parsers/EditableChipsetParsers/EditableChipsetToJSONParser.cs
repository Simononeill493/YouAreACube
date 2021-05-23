using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class EditableChipsetToJSONParser
    {
        public static string ParseEditableChipsetToJson(EditableChipset chipset)
        {
            var chipsetsJson = new ChipsetJSONData();

            foreach (var editableChipset in chipset.GetThisAndSubChipsets())
            {
                var chipsetJson = new ChipBlockJSONData(editableChipset);
                chipsetsJson.Add(chipsetJson);

                foreach(var chip in editableChipset.Chips)
                {
                    var chipJObject = _parseChipTop(chip);
                    chipsetJson.Chips.Add(chipJObject);
                }
            }

            chipsetsJson.AlphabetSort();

            var jobjectList = JToken.FromObject(chipsetsJson, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
            return jobjectList.ToString();
        }

        private static ChipJSONData _parseChipTop(ChipTop chip)
        {
            var chipJObject = new ChipJSONData(chip);

            var mappedChipType = _getChipSubMapping(chipJObject);
            _setChipTypeArguments(chipJObject, mappedChipType);
            _parseSelectedChipInputs(chipJObject);
            _setControlChipTargets(chipJObject);

            return chipJObject;
        }

        public static GraphicalChipData _getChipSubMapping(ChipJSONData chipJObject)
        {
            if (chipJObject.ChipData.IsMappedToSubChips)
            {
                var selectedTypes = chipJObject.ChipTop.GetSelectedInputTypes();
                var mappedChipType = _getFirstMatchingMapping(selectedTypes, chipJObject.ChipData.InputMappings);

                chipJObject.ActualChipType = mappedChipType.Name;
                return mappedChipType;
            }

            return null;
        }

        public static void _setChipTypeArguments(ChipJSONData chipJObject, GraphicalChipData mappedChipType)
        {
            var chip = chipJObject.ChipTop;

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
            var inputsList = chipJObject.ChipTop.GetCurrentInputs();
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
            if(chipJObject.ChipData.Name.Equals("If"))
            {
                var ifChip = (ChipTopSwitch)chipJObject.ChipTop;

                chipJObject.Yes = ifChip.SwitchChipsets[0].Name;
                chipJObject.No = ifChip.SwitchChipsets[1].Name;

            }
            if (chipJObject.ChipData.Name.Equals("KeySwitch"))
            {
                var keyChip = (ChipTopSwitch)chipJObject.ChipTop;

                chipJObject.KeyEffects = new List<(string, string)>();

                foreach (var keyAndChipset in keyChip.GetSwitchSectionsWithNames())
                {
                    chipJObject.KeyEffects.Add((keyAndChipset.Item1, keyAndChipset.Item2.Name));
                }
            }
        }

        public static GraphicalChipData _getFirstMatchingMapping(List<string> inputs, List<GraphicalChipData> possibleMatches)
        {
            foreach (var possibleMatch in possibleMatches)
            {
                for (int i = 0; i < inputs.Count; i++)
                {
                    if (TemplateEditUtils.IsValidInputFor(inputs[i], possibleMatch.Inputs[i]))
                    {
                        return possibleMatch;
                    }
                }
            }

            return null;
        }
    }
}
