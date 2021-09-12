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
    }
}
