using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class Parser_JSONToBlockset
    {
        public static Blockset ParseJsonToBlockset(string json, IBlocksetTopLevelContainer container)
        {
            var fullJSON = JsonConvert.DeserializeObject<FullChipsetJSONData>(json);

            fullJSON.SetBlockData();
            fullJSON.CreateBlocksetObjects(container);
            fullJSON.AppendBlocksToBlocksets();
            fullJSON.MakeDicts();

            foreach (var chip in fullJSON.ChipsDict.Values)
            {
                _setBlockInputs(chip, fullJSON.ChipsDict);
                _setControlBlockTargets(chip, fullJSON.ChipsetsDict);
            }

            var baseBlockset = fullJSON.GetInitial().Blockset;
            baseBlockset.AddAndRemoveQueuedChildren_Cascade();
            baseBlockset.AssertSanityTest();

            return baseBlockset;
        }

        private static void _setControlBlockTargets(ChipJSONData chipJSON, Dictionary<string, ChipsetJSONData> chipsets)
        {
            if (chipJSON.BlockData.Name.Equals("If"))
            {
                var ifBlock = (BlockTopSwitch)chipJSON.Block;

                ifBlock.AddSwitchSection("Yes", chipsets[chipJSON.Yes].Blockset);
                ifBlock.AddSwitchSection("No", chipsets[chipJSON.No].Blockset);
            }
            if (chipJSON.BlockData.Name.Equals("KeySwitch"))
            {
                var keySwitchBlock = (BlockTopSwitch)chipJSON.Block;

                foreach (var (keyString, blockName) in chipJSON.KeyEffects)
                {
                    var block = chipsets[blockName].Blockset;
                    keySwitchBlock.AddSwitchSection(keyString, block);
                }
            }
        }
        
        private static void _setBlockInputs(ChipJSONData chipJSON, Dictionary<string, ChipJSONData> chipsDict)
        {
            for (int i = 0; i < chipJSON.Inputs.Count; i++)
            {
                _setBlockInput(chipJSON, chipsDict,i);
            }
        }
        private static void _setBlockInput(ChipJSONData chipJSON,Dictionary<string,ChipJSONData> chipsDict,int inputIndex)
        {
            var inputDataJSON = chipJSON.Inputs[inputIndex];
            var inputOption = _parseInputOption(chipJSON,chipsDict,inputDataJSON,inputIndex);

            chipJSON.Block.ManuallySetInputSection(inputOption, inputIndex);
        }

        private static BlockInputOption _parseInputOption(ChipJSONData chipJSON, Dictionary<string, ChipJSONData> chipsDict, ChipJSONInputData inputData, int inputIndex)
        {
            switch (inputData.InputType)
            {
                case InputOptionType.Value:
                    return _parseValueChipInput(chipJSON, inputIndex);
                case InputOptionType.Reference:
                    return _parseReferenceChipInput(chipsDict, inputData.InputValue);
                case InputOptionType.Variable:
                    return _parseVariableChipInput(int.Parse(inputData.InputValue));
            }

            throw new Exception("Unrecognized chip input option type");
        }
        private static BlockInputOption _parseReferenceChipInput(Dictionary<string, ChipJSONData> chipsDict, string inputValue)
        {
            var inputtingChip = (BlockTopWithOutput)chipsDict[inputValue].Block;
            return new BlockInputOptionReference(inputtingChip);
        }
        private static BlockInputOption _parseValueChipInput(ChipJSONData chip, int inputIndex)
        {
            var value = chip.ParseInput(inputIndex);
            return new BlockInputOptionValue(value);
        }
        private static BlockInputOption _parseVariableChipInput(int variableIndex)
        {
            return new BlockInputOptionVariable(new TemplateVariable(variableIndex,"_nameNotSet_", new InGameType(TemplateEditUtils.PlaceholderType)));
        }

    }
}
