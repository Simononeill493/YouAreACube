using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class BlockDataDatabase
    {
        public static Dictionary<string,BlockData> BlockDataDict;
        public static IEnumerable<BlockData> BaseBlockDataList;

        public static void Load()
        {
            BlockDataDict = _loadBlockDataDict();
            BaseBlockDataList = BlockDataDict.Values.Where(c => c.BaseMappingBlock == null);
        }

        private static Dictionary<string, BlockData> _loadBlockDataDict()
        {
            var data = FileUtils.LoadJson(ConfigFiles.GraphicalChipsPath)["chips"];
            var blockData =  BlockDataParser.ParseBlockDatas(data);

            foreach (var chip in blockData.Values.ToList())
            {
                foreach(var subMapping in chip.GetAllSubMappings())
                {
                    blockData[subMapping.Name] = subMapping;
                    subMapping.Init();
                    subMapping.BaseMappingBlock = chip;
                }
            }

            return blockData;
        }

        public static IEnumerable<BlockData> SearchBlocks(this IEnumerable<BlockData> blocks, string searchTerm) => blocks.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()));
    }
}
