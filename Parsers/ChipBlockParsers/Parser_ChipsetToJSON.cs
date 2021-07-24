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
            var fullChipsetJSON = new FullChipsetJSONData();
            var chipsDict = new Dictionary<string, ChipJSONData>();

            foreach (var chip in chipsetToParse.GetAllChipsAndSubChips())
            {
                var chipJSON = new ChipJSONData(chip);
                chipsDict[chip.Name] = chipJSON;

                _setGenericChipAttributes(chipJSON);
                _setControlChipAttributes(chipJSON);
            }

            _setChipReferenceInputs(chipsDict);
            _setChipValueInputs(chipsDict);

            foreach (var chipset in chipsetToParse.GetChipsetAndSubChipsets())
            {
                var chipsetJSON = new ChipsetJSONData(chipset);
                chipsetJSON.Chips = _getChipsInThisChipset(chipset,chipsDict);

                fullChipsetJSON.Add(chipsetJSON);
            }

            fullChipsetJSON.AlphabetSort();

            var jobjectList = JToken.FromObject(fullChipsetJSON, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
            return jobjectList.ToString();
        }


        private static void _setChipReferenceInputs(Dictionary<string, ChipJSONData> chipsDict)
        {
            foreach (var chipJSON in chipsDict.Values)
            {
                if (chipJSON.GraphicalChipData.HasOutput)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var targets = chipJSON.Chip.GetTargetsList(i);
                        _setChipReferenceTargets(chipJSON.Chip, i, targets, chipsDict);
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
        private static void _setChipValueInputs(Dictionary<string, ChipJSONData> chipsJsonData)
        {
            foreach (var chipJobject in chipsJsonData.Values)
            {
                var inputs = chipJobject.Inputs;

                for (int i = 0; i < inputs.Count; i++)
                {
                    if (inputs[i].InputType.Equals(""))
                    {
                        inputs[i].InputType = "Value";
                        inputs[i].InputValue = _getInputPinValue(chipJobject.Chip, i);
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
                var ifChip = (IfChip)chipJObject.Chip;

                chipJObject.Yes = ifChip.Yes.Name;
                chipJObject.No = ifChip.No.Name;
            }
            else if (chipJObject.GraphicalChipData.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (KeySwitchChip)chipJObject.Chip;
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
                var type = chipToken.Chip.GetType();
                var typeArguments = type.GenericTypeArguments;
                chipToken.TypeArguments = typeArguments.Select(ta => ta.Name).ToList();
            }
        }

        private static List<ChipJSONData> _getChipsInThisChipset(Chipset chipset, Dictionary<string, ChipJSONData> chipJObjects)
        {
            return chipset.Chips.Select(chip => chipJObjects[chip.Name]).ToList();
        }
    }
}
