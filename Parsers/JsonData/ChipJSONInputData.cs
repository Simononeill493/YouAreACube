using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{

    public class ChipJSONInputData
    {
        public string InputType;
        public string InputValue;

        public ChipJSONInputData(string inputType, string inputValue)
        {
            InputType = inputType;
            InputValue = inputValue;
        }

        public object Parse(string typeName)
        {
            if(!InputType.Equals("Value"))
            {
                throw new Exception();
            }

            if (typeName.Equals(nameof(BlockTemplate)))
            {
                var splits = InputValue.Split('|');
                if (splits.Length != 2)
                {
                    throw new Exception("Template chip input parsing error: " + InputValue);
                }

                var name = splits[0];
                var version = int.Parse(splits[1]);
                var template = Templates.Database[name][version];
                return template;
            }


            var type = TypeUtils.GetTypeByDisplayName(typeName);
            var typeValue = TypeUtils.ParseType(type, InputValue);

            if(typeValue==null)
            {
                throw new Exception();
            }

            return typeValue;
        }
    }
}
