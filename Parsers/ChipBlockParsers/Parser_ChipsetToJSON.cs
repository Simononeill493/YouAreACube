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
        public static string ToJson(this Chipset chipsetToParse)
        {
            chipsetToParse.AssertSanityTest();

            var fullJSON = new ChipsetModeJSONData(chipsetToParse);

            _setChipsInputs(fullJSON.ChipsDict);

            fullJSON.AlphabetSort();
            var asString = fullJSON.GenerateString();
            return asString;
        }

        private static void _setChipsInputs(Dictionary<string, ChipJSONData> chipsDict)
        {
            foreach (var chipJson in chipsDict.Values)
            {
                chipJson.Inputs = new List<ChipJSONInputData>();
                _setChipInputs(chipJson, chipJson.Chip);
            }
        }

        private static void _setChipInputs(ChipJSONData chipJson,IChip chip)
        {
            var chipInputPinData = chip.GetInputPinValues();

            for(int i = 0;i<chipInputPinData.List.Count;i++)
            {
                var inputPinData = chipInputPinData.List[i];
                var inputData = _generateInputData(inputPinData.Item1, inputPinData.Item2, i);
                chipJson.Inputs.Add(inputData);
            }
        }

        private static ChipJSONInputData _generateInputData(InputOptionType inputType,object value,int pin)
        {
            switch (inputType)
            {
                case InputOptionType.Value:
                    return new ChipJSONInputData(inputType, JSONDataUtils.ObjectToJSONRep(value), pin);
                case InputOptionType.Reference:
                    var referenceChip = (IChip)value;
                    return new ChipJSONInputData(inputType, referenceChip.Name, pin);
                case InputOptionType.Variable:
                    var inputData =  new ChipJSONInputData(inputType, value.ToString(),pin);
                    return inputData;
                default:
                    throw new Exception();
            }
        }

    }
}
