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
    public static class Parser_ChipsetToJSON
    {
        public static string ParseChipsetToJson(Chipset chipsetToParse)
        {
            var chipsetsJson = new FullChipsetJSONData();
            var chipNamesToJsonData = new Dictionary<string, ChipJSONData>();

            foreach (var iChip in chipsetToParse.GetAllChipsAndSubChips())
            {
                var chipJsonData = new ChipJSONData(iChip);
                chipNamesToJsonData[iChip.Name] = chipJsonData;

                _setGenericChipAttributes(chipJsonData);
                _setControlChipAttributes(chipJsonData);
            }

            _setChipReferenceInputs(chipNamesToJsonData);
            _setChipValueInputs(chipNamesToJsonData.Values);

            foreach (var chipset in chipsetToParse.GetChipsetAndSubChipsets())
            {
                var chipsetJsonData = new ChipsetJSONData(chipset);
                chipsetJsonData.Chips = _getChipsInThisChipset(chipset,chipNamesToJsonData);

                chipsetsJson.Add(chipsetJsonData);
            }

            chipsetsJson.AlphabetSort();

            var jobjectList = JToken.FromObject(chipsetsJson, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
            return jobjectList.ToString();
        }

        private static List<ChipJSONData> _getChipsInThisChipset(Chipset block, Dictionary<string, ChipJSONData> chipJObjects)
        {
            return block.Chips.Select(chip => chipJObjects[chip.Name]).ToList();
        }

        private static void _setChipReferenceInputs(Dictionary<string, ChipJSONData> chipsJsonData)
        {
            foreach (var chipJobject in chipsJsonData.Values)
            {
                if (chipJobject.GraphicalChipData.HasOutput)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var targets = chipJobject.IChip.GetTargetsList(i);
                        _setChipReferenceTargets(chipJobject.IChip, i, targets, chipsJsonData);
                    }
                }
            }
        }
        private static void _setChipReferenceTargets(IChip chip, int inputPinIndex, IList targets, Dictionary<string, ChipJSONData> chipsJsonData)
        {
            foreach (var target in (IEnumerable<IChip>)targets)
            {
                var targetInputs = chipsJsonData[target.Name].Inputs;
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
            if(value.GetType() == typeof(CubeTemplate))
            {
                var template = (CubeTemplate)value;
                return template.Versions.Name + '|' + template.Version;
            }
            return value.ToString();
        }
        private static void _setControlChipAttributes(ChipJSONData chipJObject)
        {
            if (chipJObject.GraphicalChipData.Name.Equals("If"))
            {
                var ifChip = (IfChip)chipJObject.IChip;

                chipJObject.Yes = ifChip.Yes.Name;
                chipJObject.No = ifChip.No.Name;
            }
            else if (chipJObject.GraphicalChipData.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (KeySwitchChip)chipJObject.IChip;
                var keysAndEffects = new List<(string, string)>();

                foreach (var keyBlock in keySwitchChip.KeyEffects)
                {
                    keysAndEffects.Add((keyBlock.Key.ToString(), keyBlock.Chipset.Name));
                }

                chipJObject.KeyEffects = keysAndEffects;
            }
        }
        private static void _setGenericChipAttributes(ChipJSONData chipToken)
        {
            if (chipToken.GraphicalChipData.IsGeneric)
            {
                var type = chipToken.IChip.GetType();
                var typeArguments = type.GenericTypeArguments;
                chipToken.TypeArguments = typeArguments.Select(ta => ta.Name).ToList();
            }
        }
    }
}
