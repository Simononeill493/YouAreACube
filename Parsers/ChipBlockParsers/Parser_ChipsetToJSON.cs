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
            var chipsDict = chipsetToParse.GetAllChipsAndSubChips().Select(c => new ChipJSONData(c)).ToDict();
            var fullJSON = new FullChipsetJSONData(chipsetToParse, chipsDict);

            _setReferencesForChipsWithOutputs(chipsDict);
            _setChipValueInputs(chipsDict);

            fullJSON.AlphabetSort();
            return fullJSON.GenerateString();
        }

        private static void _setReferencesForChipsWithOutputs(Dictionary<string, ChipJSONData> chipsDict)
        {
            foreach (var chipJSON in chipsDict.ChipsWithOutput())
            {
                for (int i = 0; i < Config.NumChipInputPins; i++)
                {
                    var targetChips = chipJSON.Chip.GetTargetsList(i);
                    _setChipReferenceInputs(chipJSON.Chip, i, targetChips, chipsDict);
                }
            }
        }
        private static void _setChipReferenceInputs(IChip sourceChip, int inputPinIndex, IList targetChips, Dictionary<string, ChipJSONData> chipsDict)
        {
            foreach (var targetChip in (IEnumerable<IChip>)targetChips)
            {
                var targetInputs = chipsDict[targetChip.Name].Inputs;
                targetInputs[inputPinIndex] = new ChipJSONInputData(InputOptionType.Reference, sourceChip.Name,inputPinIndex);
            }
        }

        private static void _setChipValueInputs(Dictionary<string, ChipJSONData> chipsDict)
        {
            foreach (var (undefinedPin,chipJSON) in chipsDict.GetPinsWithUndefinedInput())
            {
                undefinedPin.InputType = InputOptionType.Value;
                undefinedPin.InputValue = chipJSON.Chip.GetInputPinValue(undefinedPin.PinIndex);
            }
        }
    }
}
