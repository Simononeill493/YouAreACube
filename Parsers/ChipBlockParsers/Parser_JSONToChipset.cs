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
            var chipsetsJson = JsonConvert.DeserializeObject<ChipsetJSONData>(json);
            var blocksDict = chipsetsJson.GetBlocksDict();
            var chipsDict = chipsetsJson.GetChipsDict();

            chipsetsJson.SetChipData();
            chipsetsJson.CreateChipBlockObjects();

            _appendChipsToChipBlocks(chipsetsJson);

            foreach (var chipToken in chipsDict.Values)
            {
                _setChipInputs(chipToken, chipsDict);
                _setControlChipTargets(chipToken, blocksDict);
            }

            var initBlock = chipsetsJson.GetInitial().ChipBlock;
            return initBlock;
        }

        private static void _appendChipsToChipBlocks(ChipsetJSONData chipsetsJson)
        {
            foreach (var blockToken in chipsetsJson)
            {
                foreach (var chipToken in blockToken.Chips)
                {
                    blockToken.ChipBlock.AddChip(chipToken.IChip);
                }
            }
        }
        private static void _setControlChipTargets(ChipJSONData chipToken, Dictionary<string, ChipBlockJSONData> blocksDict)
        {
            var chipData = chipToken.GraphicalChipData;
            var constructedChip = chipToken.IChip;

            if (chipData.Name.Equals("If"))
            {
                var ifChip = (IfChip)constructedChip;

                ifChip.Yes = blocksDict[chipToken.Yes].ChipBlock;
                ifChip.No = blocksDict[chipToken.No].ChipBlock;
            }
            if (chipData.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (KeySwitchChip)constructedChip;

                foreach (var (keyString, blockName) in chipToken.KeyEffects)
                {
                    var effectKey = (Keys)Enum.Parse(typeof(Keys), keyString);
                    var effectBlock = blocksDict[blockName].ChipBlock;

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
            var inputChip = chipsDict[inputChipName].IChip;
            inputChip.AddTarget(inputIndex, chipToken.IChip);
        }
        private static void _setValueChipInput(ChipJSONData chipToken, int inputIndex)
        {
            var currentIChip = chipToken.IChip;
            currentIChip.SetInputProperty(inputIndex,chipToken.ParseInput(inputIndex));
        }
    }
}
