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

            var chipsetsDict = fullJSON.GetChipsetsDict();
            var chipsDict = fullJSON.GetChipsDict();

            foreach (var chipToken in chipsDict.Values)
            {
                _setChipInputs(chipToken, chipsDict);
                _setControlChipTargets(chipToken, chipsetsDict);
            }

            var baseChipset = fullJSON.GetInitial().Chipset;

            baseChipset.AssertSanityTest();
            return baseChipset;
        }

        private static void _setControlChipTargets(ChipJSONData chipJSON, Dictionary<string, ChipsetJSONData> chipsetsDict)
        {
            var blockData = chipJSON.BlockData;
            var constructedChip = chipJSON.Chip;

            if (blockData.Name.Equals("If"))
            {
                var ifChip = (IfChip)constructedChip;

                ifChip.Yes = chipsetsDict[chipJSON.Yes].Chipset;
                ifChip.No = chipsetsDict[chipJSON.No].Chipset;
            }
            if (blockData.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (KeySwitchChip)constructedChip;

                foreach (var (keyString, blockName) in chipJSON.KeyEffects)
                {
                    var effectKey = (Keys)Enum.Parse(typeof(Keys), keyString);
                    var effectBlock = chipsetsDict[blockName].Chipset;

                    keySwitchChip.AddKeyEffect(effectKey, effectBlock);
                }
            }
        }

        private static void _setChipInputs(ChipJSONData chipToken, Dictionary<string, ChipJSONData> chipsDict)
        {
            for (int i = 0; i < chipToken.BlockData.NumInputs; i++)
            {
                _setChipInput(chipToken, chipsDict, i);
            }
        }
        private static void _setChipInput(ChipJSONData chipToken, Dictionary<string, ChipJSONData> chipsDict,int inputIndex)
        {
            var input = chipToken.Inputs[inputIndex];
            if (input.InputType == InputOptionType.Value)
            {
                _setValueChipInput(chipToken, inputIndex);
            }
            else if (input.InputType == InputOptionType.Reference)
            {
                _setReferenceChipInput(chipToken, chipsDict, input.InputValue, inputIndex);
            }
            else
            {
                throw new Exception("Unrecognized chip input option type");
            }
        }
        
        private static void _setReferenceChipInput(ChipJSONData chipToken,Dictionary<string, ChipJSONData> chipsDict, string inputChipName, int inputIndex)
        {
            var chip = chipsDict[inputChipName].Chip;
            chip.AddTarget(inputIndex, chipToken.Chip);
        }
        private static void _setValueChipInput(ChipJSONData chipToken, int inputIndex)
        {
            chipToken.Chip.SetInputProperty(inputIndex,chipToken.ParseInput(inputIndex));
        }
    }
}
