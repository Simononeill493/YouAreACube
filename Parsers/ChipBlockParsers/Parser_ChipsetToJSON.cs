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
            var chipInputPinData = chip.GetInputPinValues(chipJson.BlockData);

            for(int i = 0;i<chipInputPinData.Count;i++)
            {
                var inputPinData = chipInputPinData[i];
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
