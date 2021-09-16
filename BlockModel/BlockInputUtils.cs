using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class BlockInputUtils
    {
        public static List<BlockInputOption> GetInputsFromVariables(this IVariableProvider variableProvider, List<string> inputTypes)
        {
            var variables = variableProvider.GetVariables().Dict.Values;
            var matching = variables.Where(v => InGameTypeUtils.IsValidInputFor(v.VariableType.Name, inputTypes));

            return matching.Select(v => BlockInputOption.CreateVariable(v)).ToList();
        }

        public static List<BlockInputOption> GetMetaInputsFromVariables(this IVariableProvider variableProvider)
        {
            var variables = variableProvider.GetVariables().Dict.Values;

            return variables.Select(v => BlockInputOption.CreateMetaVariable(v.VariableNumber)).ToList();
        }

        public static List<BlockInputOption> GetInputsFromChipsets(this FullModel fullModel)
        {
            var chips = fullModel.Blocksets.Values.Where(b => !b.Internal);

            return chips.Select(c => BlockInputOption.CreateChipset(c)).ToList();
        }
    }

    [Serializable()]
    public enum BlockSpecialInputType
    {
        None,
        MetaVariable,
        Chipset
    }
}
