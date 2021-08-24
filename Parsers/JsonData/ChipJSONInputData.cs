using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipJSONInputData
    {
        public InputOptionType InputType;
        public string InputValue;
        public int PinIndex;

        public ChipJSONInputData(InputOptionType inputType, string inputValue,int pinIndex)
        {
            InputType = inputType;
            InputValue = inputValue;
            PinIndex = pinIndex;
        }

        public object Parse(string typeName)
        {
            if(InputType != InputOptionType.Value)
            {
                throw new Exception("Trying to parse chip input but is not a value type.");
            }

            return JSONDataUtils.JSONRepToObject(InputValue, typeName);
        }

        public static List<ChipJSONInputData> GenerateInputsFromBlock(BlockTop block)
        {
            var inputs = new List<ChipJSONInputData>();
            var inputsList = block.GetCurrentInputs();
            for (int i = 0; i < inputsList.Count; i++)
            {
                inputs.Add(GenerateInputFromBlockSection(inputsList[i], i));
            }

            return inputs;
        }
        public static ChipJSONInputData GenerateInputFromBlockSection(BlockInputOption blockInputOption, int pinIndex)
        {
            var inputOptionType = blockInputOption.OptionType;
            if (inputOptionType == InputOptionType.Parseable)
            {
                inputOptionType = InputOptionType.Value;
            }

            var jsonData = new ChipJSONInputData(inputOptionType, blockInputOption.ToJSONRep(), pinIndex);

            return jsonData;
        }
    }
}
