using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockDataParser
    {
        public static Dictionary<string, BlockData> ParseBlockDatas(JToken blockDatas)
        {
            var blockDataDict = new Dictionary<string, BlockData>();

            foreach (var data in JsonConvert.DeserializeObject<List<BlockData>>(blockDatas.ToString()))
            {
                data.Init();
                blockDataDict[data.Name] = data;
            }

            return blockDataDict;
        }
    }
}
