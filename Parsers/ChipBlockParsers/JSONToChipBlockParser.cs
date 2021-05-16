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
    public static class JSONToChipBlockParser
    {
        public static ChipBlock ParseJsonToBlock(string json)
        {
            var chipsetJson = JsonConvert.DeserializeObject<ChipsetJSONData>(json);
            var blocksDict = chipsetJson.GetBlocksDict();
            var chipsDict = chipsetJson.GetChipsDict();

            chipsetJson.SetChipData();
            chipsetJson.CreateChipBlocks();
            chipsetJson.CreateIChips();

            foreach (var blockToken in chipsetJson)
            {
                foreach (var chipToken in blockToken.Chips)
                {
                    blockToken.ChipBlock.AddChip(chipToken.IChip);
                }
            }

            foreach (var chipToken in chipsDict.Values)
            {
                var chipData = chipToken.ChipData;
                var constructedChip = chipToken.IChip;

                if (chipData.NumInputs > 0)
                {
                    _setChipInputs(chipToken, chipsDict);
                }

                if(chipData.ChipDataType == ChipType.Control)
                {
                    _setControlChipTargets(chipToken, blocksDict);
                }
            }

            var initBlock = blocksDict.Values.First(v => v.Name.Equals("_Initial")).ChipBlock;
            return initBlock;
        }
        
        private static void _setChipInputs(ChipJSONData chipToken, Dictionary<string, ChipJSONData> chipsDict)
        {
            var chipData = chipToken.ChipData;
            var constructedChip = chipToken.IChip;

            var inputsList = chipToken.Inputs;
            for (int i = 0; i < chipData.NumInputs; i++)
            {
                var input = inputsList[i];
                if (input.InputType.Equals("Value"))
                {
                    var typeName = chipData.GetInputType(i);
                    var property = constructedChip.GetType().GetProperty("ChipInput" + (i + 1).ToString());

                    if (typeName.Equals(nameof(BlockTemplate)))
                    {
                        var splits = input.InputValue.Split('|');
                        var name = splits[0];
                        var version = splits[1];

                        property.SetValue(constructedChip, Templates.Database[name][int.Parse(version)]);
                    }
                    else
                    {
                        var type = TypeUtils.GetTypeByDisplayName(typeName);
                        var typeValue = TypeUtils.ParseType(type, input.InputValue);
                        property.SetValue(constructedChip, typeValue);
                    }
                }
                else if (input.InputType.Equals("Reference"))
                {
                    var inputChip = chipsDict[input.InputValue].IChip;
                    var inputChipType = inputChip.GetType();
                    var targetField = inputChipType.GetField("Targets" + (i + 1).ToString());
                    IList targetsList = (IList)targetField.GetValue(inputChip);
                    targetsList.Add(constructedChip);
                }
            }
        }
        private static void _setControlChipTargets(ChipJSONData chipToken, Dictionary<string, ChipBlockJSONData> blocksDict)
        {
            var chipData = chipToken.ChipData;
            var constructedChip = chipToken.IChip;

            if (chipData.Name.Equals("If"))
            {
                var ifChip = (IfChip)constructedChip;
                var yesBLock = blocksDict[chipToken.Yes].ChipBlock;
                ifChip.Yes = yesBLock;

                var noBLock = blocksDict[chipToken.No].ChipBlock;
                ifChip.No = noBLock;
            }
            if (chipData.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (KeySwitchChip)constructedChip;
                foreach (var effect in chipToken.KeyEffects)
                {
                    var effectKey = (Keys)Enum.Parse(typeof(Keys), effect.Item1);
                    var effectBlockName = effect.Item2;
                    var effectBlock = blocksDict[effectBlockName].ChipBlock;

                    keySwitchChip.AddKeyEffect(effectKey, effectBlock);
                }
            }
        }
    }
}
