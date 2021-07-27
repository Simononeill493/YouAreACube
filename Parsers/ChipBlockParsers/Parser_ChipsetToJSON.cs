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
                chipsetJSON.Chips = _getChipsInThisChipset(chipset,chipsDict);

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
                    for (int i = 0; i < 3; i++)
                    {
                        var targets = chipJSON.Chip.GetTargetsList(i);
                        _setChipReferenceInputs(chipJSON.Chip, i, targets, chipsDict);
                    }
                }
            }
        }
        private static void _setChipReferenceInputs(IChip chip, int inputPinIndex, IList targets, Dictionary<string, ChipJSONData> chipsJsonData)
        {
            foreach (var target in (IEnumerable<IChip>)targets)
            {
                var targetInputs = chipsJsonData[target.Name].Inputs;
                targetInputs[inputPinIndex] = new ChipJSONInputData(InputOptionType.Reference, chip.Name);
            }
        }
        private static void _setChipValueInputs(Dictionary<string, ChipJSONData> chipsJsonData)
        {
            foreach (var chipJobject in chipsJsonData.Values)
            {
                var inputs = chipJobject.Inputs;

                for (int i = 0; i < inputs.Count; i++)
                {
                    if (inputs[i].InputType == InputOptionType.Undefined)
                    {
                        inputs[i].InputType = InputOptionType.Value;
                        inputs[i].InputValue = chipJobject.Chip.GetInputPinValue(i);
                    }
                }
            }
        }

        private static List<ChipJSONData> _getChipsInThisChipset(Chipset chipset, Dictionary<string, ChipJSONData> chipJObjects)
        {
            return chipset.Chips.Select(chip => chipJObjects[chip.Name]).ToList();
        }
    }
}
