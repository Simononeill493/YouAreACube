using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipParser
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

            var data = new ChipData(name, chipType);

            var inputs = chip["in"];
            if(inputs!=null)
            {
                var values = inputs.Values();
                if (values.Count() > 0) { data.Input1 = inputs[0].ToString(); }
                if (values.Count() > 1) { data.Input2 = inputs[1].ToString(); }
                if (values.Count() > 2) { data.Input3 = inputs[2].ToString(); }
            }

            var output = chip["out"];
            if(output!=null)
            {
                data.SetOutputType(output.ToString());
                data.HasOutput = true;
            }

            data.Init();
            return data;
        }
    }
}
