using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class Parser_BlocksetToJSON
    {
        public static string ParseBlocksetToJson(Blockset blocksetToParse)
        {
            blocksetToParse.AssertSanityTest();
            var fullJSON = new FullChipsetJSONData();

            foreach (var blockset in blocksetToParse.GetThisAndSubBlocksets())
            {
                var chipsetJSON = new ChipsetJSONData(blockset);
                _addBlocksToChipsetJSON(chipsetJSON, blockset.Blocks);

                fullJSON.Add(chipsetJSON);
            }

            fullJSON.AlphabetSort();
            return fullJSON.GenerateString();
        }

        private static void _addBlocksToChipsetJSON(ChipsetJSONData chipsetJSON, List<BlockTop> blocks)
        {
            foreach (var block in blocks)
            {
                var chipJSON = new ChipJSONData(block);
                chipsetJSON.Chips.Add(chipJSON);
            }
        }
    }
}
