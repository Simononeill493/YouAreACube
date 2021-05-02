using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class GraphicalChipParser
    {
        public static Dictionary<string, GraphicalChipData> ParseChips(JToken chips)
        {
            var chipsDict = new Dictionary<string, GraphicalChipData>();

            foreach (var data in JsonConvert.DeserializeObject<List<GraphicalChipData>>(chips.ToString()))
            {
                data.Init();
                chipsDict[data.Name] = data;
            }

            return chipsDict;
        }
    }
}
