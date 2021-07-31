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
            chipsetToParse.AssertSanityTest();

            var fullJSON = new FullChipsetJSONData(chipsetToParse);

            _setReferencesForChipsWithOutputs(fullJSON.ChipsDict);
            _setChipValueInputs(fullJSON.ChipsDict);

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
                    _setChipReferenceInputs(chipJSON.Chip, targetChips,i,chipsDict);
                }
            }
        }
        private static void _setChipReferenceInputs(IChip sourceChip, IList targetChips,int inputPinIndex,Dictionary<string, ChipJSONData> chipsDict)
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
            //any still undefined should be value types
            {
                undefinedPin.InputType = InputOptionType.Value;
                undefinedPin.InputValue = chipJSON.Chip.GetInputPinValue(undefinedPin.PinIndex);
            }
        }
    }
}
