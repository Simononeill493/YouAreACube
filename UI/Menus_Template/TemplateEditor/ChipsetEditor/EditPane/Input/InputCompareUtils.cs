using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class InputCompareUtils
    {
        public static List<BlockInputOption> GetInputsFromModel(this FullModel fullModel,BlockInputModel inputModel, List<string> inputTypes)
        {
            var block = fullModel.InputParents[inputModel];

            var candidates = fullModel.Blocks.Values.Where(b => InGameTypeUtils.IsValidInputFor(b.GetOutputType(), inputTypes) & !b.Equals(block)).ToList();
            var matching = candidates.Where(c => fullModel.GetAllBlocksBelow(c).Contains(block));
            return matching.Select(b => BlockInputOption.CreateReference(b)).ToList();
        }

        public static IEnumerable<BlockModel> GetAllBlocksBelow(this FullModel fullModel,BlockModel block)
        {
            var blocks = fullModel.Blocksets.Values.Where(b => b.Blocks.Contains(block)).FirstOrDefault().Blocks;
            var index = blocks.IndexOf(block);
            var allAfter = blocks.GetRange(index, blocks.Count() - index);
            var allAfterCascade = allAfter.SelectMany(a => a.GetThisAndAllSubBlocks());
            return allAfterCascade;
        }

        public static List<BlockModel> GetThisAndAllSubBlocks(this BlockModel block)
        {
            var subBlocks = block.SubBlocksets.SelectMany(s => s.Item2.Blocks.SelectMany(s2 => s2.GetThisAndAllSubBlocks())).ToList();
            subBlocks.Add(block);
            return subBlocks;
        }



    }
}
