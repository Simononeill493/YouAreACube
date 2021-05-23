using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class ChipBlockToJSONParser
    {
        public static string ParseBlockToJson(ChipBlock toParse)
        {
            var chipsetsJson = new ChipsetJSONData();
            var chipJObjects = new Dictionary<string, ChipJSONData>();
            var chipJsonInputData = new Dictionary<string, List<ChipJSONInputData>>();

            foreach (var iChip in toParse.GetAllChipsAndSubChips())
            {
                var chipJobject = new ChipJSONData(iChip);

                chipJObjects[iChip.Name] = chipJobject;
                chipJsonInputData[chipJobject.Name] = chipJobject.Inputs;

                _setGenericChipAttributes(chipJobject);
                _setControlChipAttributes(chipJobject);
            }

            _setChipReferenceInputs(chipJObjects.Values, chipJsonInputData);
            _setChipValueInputs(chipJObjects.Values);

            foreach (var block in toParse.GetBlockAndSubBlocks())
            {
                var token = new ChipBlockJSONData(block);
                token.Chips = _getJsonDataForChipsInBlock(block,chipJObjects);

                chipsetsJson.Add(token);
            }

            chipsetsJson.AlphabetSort();

            var jobjectList = JToken.FromObject(chipsetsJson, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
            return jobjectList.ToString();
        }

        private static List<ChipJSONData> _getJsonDataForChipsInBlock(ChipBlock block, Dictionary<string, ChipJSONData> chipJObjects)
        {
            return block.Chips.Select(chip => chipJObjects[chip.Name]).ToList();
        }

        private static void _setChipReferenceInputs(IEnumerable<ChipJSONData> chipsJsonData, Dictionary<string, List<ChipJSONInputData>> chipInputs)
        {
            foreach (var chipJobject in chipsJsonData)
            {
                if (chipJobject.ChipData.HasOutput)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var targets = chipJobject.IChip.GetTargetsList(i);
                        _setChipReferenceTargets(chipJobject.IChip, i, targets, chipInputs);
                    }
                }
            }
        }
        private static void _setChipReferenceTargets(IChip chip, int inputPinIndex, IList targets, Dictionary<string, List<ChipJSONInputData>> chipToInputs)
        {
            foreach (var target in (IEnumerable<IChip>)targets)
            {
                var targetInputs = chipToInputs[target.Name];
                targetInputs[inputPinIndex] = new ChipJSONInputData("Reference", chip.Name);
            }
        }
        private static void _setChipValueInputs(IEnumerable<ChipJSONData> chipsJsonData)
        {
            foreach (var chipJobject in chipsJsonData)
            {
                var inputs = chipJobject.Inputs;

                for (int i = 0; i < inputs.Count; i++)
                {
                    if (inputs[i].InputType.Equals(""))
                    {
                        inputs[i].InputType = "Value";
                        inputs[i].InputValue = _getInputPinValue(chipJobject.IChip, i);
                    }
                }
            }
        }
        private static string _getInputPinValue(IChip chip, int pinIndex)
        {
            var value = chip.GetInputPropertyValue(pinIndex);
            if(value.GetType() == typeof(BlockTemplate))
            {
                var template = (BlockTemplate)value;
                return template.Versions.Name + '|' + template.Version;
            }
            return value.ToString();
        }
        private static void _setControlChipAttributes(ChipJSONData chipJObject)
        {
            if (chipJObject.ChipData.Name.Equals("If"))
            {
                var ifChip = (IfChip)chipJObject.IChip;

                chipJObject.Yes = ifChip.Yes.Name;
                chipJObject.No = ifChip.No.Name;
            }
            else if (chipJObject.ChipData.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (KeySwitchChip)chipJObject.IChip;
                var keysAndEffects = new List<(string, string)>();

                foreach (var keyBlock in keySwitchChip.KeyEffects)
                {
                    keysAndEffects.Add((keyBlock.Key.ToString(), keyBlock.Block.Name));
                }

                chipJObject.KeyEffects = keysAndEffects;
            }
        }
        private static void _setGenericChipAttributes(ChipJSONData chipToken)
        {
            if (chipToken.ChipData.IsGeneric)
            {
                var type = chipToken.IChip.GetType();
                var typeArguments = type.GenericTypeArguments;
                chipToken.TypeArguments = typeArguments.Select(ta => ta.Name).ToList();
            }
        }
    }
}
