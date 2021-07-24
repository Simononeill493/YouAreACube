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
        public static Dictionary<string, BlockData> ParseChips(JToken chips)
        {
            var chipsDict = new Dictionary<string, BlockData>();

            foreach (var data in JsonConvert.DeserializeObject<List<BlockData>>(chips.ToString()))
            {
                data.Init();
                chipsDict[data.Name] = data;
            }

            return chipsDict;
        }
    }
}
