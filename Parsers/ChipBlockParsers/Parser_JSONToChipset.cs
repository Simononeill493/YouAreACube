using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace IAmACube
{
    public static class Parser_JSONToChipset
    {
        public static Chipset ParseJsonToBlock(string json)
        {
            var fullJSON = JsonConvert.DeserializeObject<FullChipsetJSONData>(json);

            fullJSON.SetBlockData();
            fullJSON.CreateChipsetObjects();
            fullJSON.AppendChipsToChipsets();
            fullJSON.MakeDicts();

            foreach (var chipJSON in fullJSON.ChipsDict.Values)
            {
                _setChipInputs(chipJSON, fullJSON.ChipsDict);
                _setControlChipTargets(chipJSON, fullJSON.ChipsetsDict);
            }

            var baseChipset = fullJSON.GetInitial().Chipset;

            baseChipset.AssertSanityTest();
            return baseChipset;
        }

        private static void _setControlChipTargets(ChipJSONData chipJSON, Dictionary<string, ChipsetJSONData> chipsetsDict)
        {
            var blockData = chipJSON.BlockData;

            if (blockData.Name.Equals("If"))
            {
                var ifChip = (IfChip)chipJSON.Chip;

                ifChip.Yes = chipsetsDict[chipJSON.Yes].Chipset;
                ifChip.No = chipsetsDict[chipJSON.No].Chipset;
            }
            if (blockData.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (KeySwitchChip)chipJSON.Chip;

                foreach (var (keyString, blockName) in chipJSON.KeyEffects)
                {
                    var effectKey = TypeUtils.ParseType<Keys>(keyString);
                    var effectBlock = chipsetsDict[blockName].Chipset;

                    keySwitchChip.AddKeyEffect(effectKey, effectBlock);
                }
            }
        }

        private static void _setChipInputs(ChipJSONData chipJSON, Dictionary<string, ChipJSONData> chipsDict)
        {
            for (int i = 0; i < chipJSON.BlockData.NumInputs; i++)
            {
                _setChipInput(chipJSON, chipsDict, i);
            }
        }
        private static void _setChipInput(ChipJSONData chipJSON, Dictionary<string, ChipJSONData> chipsDict,int inputIndex)
        {
            var input = chipJSON.Inputs[inputIndex];
            if (input.InputType == InputOptionType.Value)
            {
                _setValueChipInput(chipJSON, inputIndex);
                return;
            }
            else if (input.InputType == InputOptionType.Reference)
            {
                _setReferenceChipInput(chipJSON, chipsDict, input.InputValue, inputIndex);
                return;
            }

            throw new Exception("Unrecognized chip input option type");
        }
        
        private static void _setReferenceChipInput(ChipJSONData chipJSON,Dictionary<string, ChipJSONData> chipsDict, string inputChipName, int inputIndex)
        {
            var inputtingChip = chipsDict[inputChipName].Chip;
            chipJSON.Chip.SetReferenceProperty(inputtingChip,inputIndex);
        }
        private static void _setValueChipInput(ChipJSONData chipJSON, int inputIndex)
        {
            chipJSON.Chip.SetValueProperty(chipJSON.ParseInput(inputIndex), inputIndex);
        }
    }
}
