using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class Parser_ChipsetToJSON
    {
        public static string ParseChipsetToJson(Chipset chipsetToParse)
        {
            var fullJSON = new FullChipsetJSONData();
            var chipsDict = _buildChipsDict(chipsetToParse);

            _setChipReferenceTargets(chipsDict);
            _setChipValueInputs(chipsDict);

            foreach (var chipset in chipsetToParse.GetChipsetAndSubChipsets())
            {
                var chipsetJSON = new ChipsetJSONData(chipset);
                chipsetJSON.Chips = _fetchJSONDataForTheseChips(chipset.Chips,chipsDict);

                fullJSON.Add(chipsetJSON);
            }

            fullJSON.AlphabetSort();
            return fullJSON.GenerateString();
        }

        private static Dictionary<string, ChipJSONData> _buildChipsDict(Chipset chipsetToParse)
        {
            var chipsDict = new Dictionary<string, ChipJSONData>();

            foreach (var chip in chipsetToParse.GetAllChipsAndSubChips())
            {
                var chipJSON = new ChipJSONData(chip);
                chipsDict[chip.Name] = chipJSON;
            }

            return chipsDict;
        }

        private static void _setChipReferenceTargets(Dictionary<string, ChipJSONData> chipsDict)
        {
            foreach (var chipJSON in chipsDict.Values)
            {
                if (chipJSON.BlockData.HasOutput)
                {
                    for (int i = 0; i < Config.NumChipInputPins; i++)
                    {
                        var targetChips = chipJSON.Chip.GetTargetsList(i);
                        _setChipReferenceInputs(chipJSON.Chip, i, targetChips, chipsDict);
                    }
                }
            }
        }
        private static void _setChipReferenceInputs(IChip sourceChip, int inputPinIndex, IList targetChips, Dictionary<string, ChipJSONData> chipsJSON)
        {
            foreach (var targetChip in (IEnumerable<IChip>)targetChips)
            {
                var targetInputs = chipsJSON[targetChip.Name].Inputs;
                targetInputs[inputPinIndex] = new ChipJSONInputData(InputOptionType.Reference, sourceChip.Name);
            }
        }

        private static void _setChipValueInputs(Dictionary<string, ChipJSONData> chipsJsonData)
        {
            foreach (var chipJSON in chipsJsonData.Values)
            {
                var inputs = chipJSON.Inputs;
                for (int i = 0; i < inputs.Count; i++)
                {
                    _setChipValueInput(inputs[i], chipJSON, i);
                }
            }
        }
        private static void _setChipValueInput(ChipJSONInputData inputData, ChipJSONData chipJSON, int index)
        {
            if (inputData.InputType == InputOptionType.Undefined)
            //Chip input pin hasn't been set by a reference to another chip, so it must be a static value
            {
                inputData.InputType = InputOptionType.Value;
                inputData.InputValue = chipJSON.Chip.GetInputPinValue(index);
            }
        }

        private static List<ChipJSONData> _fetchJSONDataForTheseChips(List<IChip> chips, Dictionary<string, ChipJSONData> chipJObjects)
        {
            return chips.Select(chip => chipJObjects[chip.Name]).ToList();
        }
    }
}
