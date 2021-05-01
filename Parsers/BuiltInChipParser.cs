using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BuiltInChipParser
    {
        public static Dictionary<string, ChipData> ParseChips(JToken chips)
        {
            var chipsDict = new Dictionary<string, ChipData>();

            foreach (var token in chips)
            {
                var chipData = ParseChip(token);
                chipsDict[chipData.Name] = chipData;
            }

            return chipsDict;
        }

        public static ChipData ParseChip(JToken chip)
        {
            var name = chip["name"].ToString();
            var typeName = chip["type"].ToString();
            var chipType = (ChipType)Enum.Parse(typeof(ChipType), typeName);

            var inputs = chip["in"];
            var inputsString = new string[3];
            int numInputs = 0;
            if (inputs != null)
            {
                foreach (var value in inputs.Values())
                {
                    inputsString[numInputs] = value.ToString();
                    numInputs++;
                }
            }

            var output = chip["out"];
            string outputString = null;
            if(output!=null)
            {
                outputString = output.ToString();
            }

            return new ChipData(name, chipType, numInputs, inputsString, outputString);
        }
    }
}
