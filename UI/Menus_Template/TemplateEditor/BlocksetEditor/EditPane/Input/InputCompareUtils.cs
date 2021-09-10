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

            var candidates = fullModel.Blocks.Values.Where(b => InGameTypeUtils.IsValidInputFor(b.GetCurrentOutputType(), inputTypes) & !b.Equals(block)).ToList();
            var matching = candidates.Where(c => fullModel.GetAllBlocksBelow(c).Contains(block));
            return matching.Select(b => BlockInputOption.CreateReference(b)).ToList();
        }

        public static List<BlockModel> GetAllBlocksBelow(this FullModel fullModel,BlockModel block)
        {
            var blocks = fullModel.Blocksets.Values.Where(b => b.Blocks.Contains(block)).FirstOrDefault().Blocks;
            var index = blocks.IndexOf(block);
            var allAfter = blocks.GetRange(index, blocks.Count() - index);
            return allAfter;
        }

        public static List<BlockInputOption> GetInputsFromVariables(this IVariableProvider variableProvider,List<string> inputTypes)
        {
            var variables = variableProvider.GetVariables().Dict.Values;
            var matching = variables.Where(v => InGameTypeUtils.IsValidInputFor(v.VariableType.Name, inputTypes));

            return matching.Select(v => BlockInputOption.CreateVariable(v)).ToList();
        }
    }
}
