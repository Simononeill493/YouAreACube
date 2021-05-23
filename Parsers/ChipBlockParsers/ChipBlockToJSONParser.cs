using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
            var chipToObject = new Dictionary<string, ChipJSONData>();
            var chipToInputs = new Dictionary<string, List<ChipJSONInputData>>();

            foreach (var iChip in toParse.GetAllChipsAndSubChips())
            {
                var chipData = iChip.GetChipData();

                var chipJobject = new ChipJSONData();
                chipJobject.Name = iChip.Name;
                chipJobject.GraphicalChipType = chipData.BaseMappingName;
                chipJobject.ActualChipType = chipData.Name;

                chipJobject.ChipData = chipData;
                chipJobject.IChip = iChip;
                chipJobject.SetInputs();
                chipToInputs[chipJobject.Name] = chipJobject.Inputs;

                chipToObject[iChip.Name] = chipJobject;

                _setGenericChipAttributes(iChip, chipJobject, chipData);
                _setControlChipAttributes(iChip, chipJobject, chipData);
            }

            _setChipReferenceInputs(chipToObject.Values, chipToInputs);
            _setChipValueInputs(chipToObject.Values);

            var blocksJsonData = new ChipsetJSONData();
            foreach (var block in toParse.GetBlockAndSubBlocks())
            {
                var token = new ChipBlockJSONData();
                token.Name = block.Name;
                token.Chips = block.Chips.Select(chip => chipToObject[chip.Name]).ToList();

                blocksJsonData.Add(token);
            }

            blocksJsonData.Sort((c1, c2) => string.Compare(c1.Name, c2.Name));

            var jobjectList = JToken.FromObject(blocksJsonData, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
            return jobjectList.ToString();
        }

        private static void _setChipReferenceInputs(IEnumerable<ChipJSONData> chipsJsonData, Dictionary<string, List<ChipJSONInputData>> chipInputs)
        {
            foreach (var chipJobject in chipsJsonData)
            {
                var chipData = chipJobject.ChipData;
                var chipType = chipJobject.IChip.GetType();

                if (chipData.HasOutput)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var targets = (IEnumerable<IChip>)chipType.GetField("Targets" + (i + 1)).GetValue(chipJobject.IChip);
                        _setChipReferenceTargets(chipJobject.IChip, i, targets, chipInputs);
                    }
                }
            }
        }
        private static void _setChipReferenceTargets(IChip chip, int inputPinIndex, IEnumerable<IChip> targets, Dictionary<string, List<ChipJSONInputData>> chipToInputs)
        {
            foreach (var target in targets)
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
            var propertyName = "ChipInput" + (pinIndex + 1).ToString();
            var property = chip.GetType().GetProperty(propertyName);

            var value = property.GetValue(chip);
            if(value.GetType() == typeof(BlockTemplate))
            {
                var template = (BlockTemplate)value;
                return template.Versions.Name + '|' + template.Version;
            }
            return value.ToString();
        }
        private static void _setControlChipAttributes(IChip chip, ChipJSONData chipJObject, GraphicalChipData data)
        {
            if (data.Name.Equals("If"))
            {
                var ifChip = (IfChip)chip;
                chipJObject.Yes = ifChip.Yes.Name;
                chipJObject.No = ifChip.No.Name;
            }
            else if (data.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (KeySwitchChip)chip;
                var keysAndEffects = new List<(string, string)>();
                foreach (var keyBlock in keySwitchChip.KeyEffects)
                {
                    keysAndEffects.Add((keyBlock.Key.ToString(), keyBlock.Block.Name));
                }

                chipJObject.KeyEffects = keysAndEffects;
            }
        }
        private static void _setGenericChipAttributes(IChip chip, ChipJSONData chipToken, GraphicalChipData data)
        {
            if (data.IsGeneric)
            {
                var type = chip.GetType();
                var typeArguments = type.GenericTypeArguments;
                chipToken.TypeArguments = typeArguments.Select(ta => ta.Name).ToList();
            }
        }
    }
}
