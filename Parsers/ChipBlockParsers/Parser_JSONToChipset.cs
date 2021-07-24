using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class Parser_JSONToChipset
    {
        public static Chipset ParseJsonToBlock(string json)
        {
            var fullChipsetJSON = JsonConvert.DeserializeObject<FullChipsetJSONData>(json);
            var chipsetsDict = fullChipsetJSON.GetChipsetsDict();
            var chipsDict = fullChipsetJSON.GetChipsDict();

            fullChipsetJSON.SetChipData();
            fullChipsetJSON.CreateChipsetObjects();

            _appendChipsToChipsets(fullChipsetJSON);

            foreach (var chipToken in chipsDict.Values)
            {
                _setChipInputs(chipToken, chipsDict);
                _setControlChipTargets(chipToken, chipsetsDict);
            }

            return fullChipsetJSON.GetInitial().Chipset;
        }

        private static void _appendChipsToChipsets(FullChipsetJSONData chipsetsJson)
        {
            foreach (var chipsetJSON in chipsetsJson)
            {
                foreach (var chipJSON in chipsetJSON.Chips)
                {
                    chipsetJSON.Chipset.AddChip(chipJSON.Chip);
                }
            }
        }
        private static void _setControlChipTargets(ChipJSONData chipJSON, Dictionary<string, ChipsetJSONData> chipsetsDict)
        {
            var chipData = chipJSON.GraphicalChipData;
            var constructedChip = chipJSON.Chip;

            if (chipData.Name.Equals("If"))
            {
                var ifChip = (IfChip)constructedChip;

                ifChip.Yes = chipsetsDict[chipJSON.Yes].Chipset;
                ifChip.No = chipsetsDict[chipJSON.No].Chipset;
            }
            if (chipData.Name.Equals("KeySwitch"))
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
            for (int i = 0; i < chipToken.GraphicalChipData.NumInputs; i++)
            {
                _setChipInput(chipToken, chipsDict, i);
            }
        }
        private static void _setChipInput(ChipJSONData chipToken, Dictionary<string, ChipJSONData> chipsDict,int inputIndex)
        {
            var input = chipToken.Inputs[inputIndex];
            if (input.InputType.Equals("Value"))
            {
                _setValueChipInput(chipToken, inputIndex);
            }
            else if (input.InputType.Equals("Reference"))
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
            var inputChip = chipsDict[inputChipName].Chip;
            inputChip.AddTarget(inputIndex, chipToken.Chip);
        }
        private static void _setValueChipInput(ChipJSONData chipToken, int inputIndex)
        {
            var currentIChip = chipToken.Chip;
            currentIChip.SetInputProperty(inputIndex,chipToken.ParseInput(inputIndex));
        }
    }
}
