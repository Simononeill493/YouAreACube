using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class EditableChipsetParser
    {
        #region chipsetToJson
        public static string ParseEditableChipsetToJson(EditableChipset chipset)
        {
            var chipsetsJson = new ChipsetJSONData();

            foreach(var editableChipset in chipset.GetThisAndSubChipsets())
            {
                var chipsetJson = new ChipBlockJSONData() { Chips = new List<ChipJSONData>() };
                chipsetJson.Chipset = editableChipset;
                chipsetJson.Name = editableChipset.Name;
                chipsetsJson.Add(chipsetJson);

                foreach(var chip in editableChipset.Chips)
                {
                    var chipJObject = new ChipJSONData();
                    chipJObject.ChipTop = chip;
                    chipJObject.ChipData = chip.ChipData;
                    chipJObject.Type = chip.ChipData.Name;
                    chipJObject.Name = chip.Name;
                    chipJObject.TypeArgument = chip.CurrentTypeArgument;

                    chipJObject.Inputs = new List<ChipJSONInputData>();
                    var inputsList = chip.GetCurrentInputs();
                    for(int i=0;i<inputsList.Count;i++)
                    {
                        var input = inputsList[i];
                        var inputData = new ChipJSONInputData(input.OptionType.ToString(), input.ToString());
                        chipJObject.Inputs.Add(inputData);
                    }

                    if(chipJObject.ChipData.ChipDataType == ChipType.Control)
                    {
                        _setControlChipTargets(chipJObject);
                    }

                    chipsetJson.Chips.Add(chipJObject);
                }
            }

            chipsetsJson.Sort((c1, c2) => string.Compare(c1.Name, c2.Name));

            var jobjectList = JToken.FromObject(chipsetsJson, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
            return jobjectList.ToString();
        }

        private static void _setControlChipTargets(ChipJSONData chipJObject)
        {
            var switchChip = (ChipTopSwitch)chipJObject.ChipTop;
            if(chipJObject.ChipData.Name.Equals("If"))
            {
                chipJObject.Yes = switchChip.SwitchSections["Yes"].Name;
                chipJObject.No = switchChip.SwitchSections["No"].Name;

            }
            if (chipJObject.ChipData.Name.Equals("KeySwitch"))
            {
                chipJObject.KeyEffects = new List<Tuple<string, string>>();

                foreach (var keyAndChipset in switchChip.SwitchSections)
                {
                    chipJObject.KeyEffects.Add(new Tuple<string, string>(keyAndChipset.Key, keyAndChipset.Value.Name));
                }
            }
        }
        #endregion

        #region jsonToChipset
        public static EditableChipset ParseJsonToEditableChipset(string json, IChipsetGenerator generator)
        {
            var chipsetsJson = JsonConvert.DeserializeObject<ChipsetJSONData>(json);
            var blocksDict = chipsetsJson.GetBlocksDict();
            var chipsDict = chipsetsJson.GetChipsDict();

            chipsetsJson.SetChipData();
            chipsetsJson.CreateChipsets(generator);
            chipsetsJson.CreateChipTops(generator);

            foreach (var blockJson in chipsetsJson)
            {
                foreach (var chipJson in blockJson.Chips)
                {
                    blockJson.Chipset.AppendChipToEnd(chipJson.ChipTop);
                }
            }

            foreach (var chip in chipsDict.Values)
            {
                var chipTop = chip.ChipTop;
                for (int i = 0; i < chip.Inputs.Count; i++)
                {
                    var jsonInputData = chip.Inputs[i];
                    var inputType = chip.ChipData.GetInputType(i + 1);

                    ChipInputOption inputOption = null;
                    if (jsonInputData.InputType.Equals("Reference"))
                    {
                        var referenceChip = (ChipTopWithOutput)chipsDict[jsonInputData.InputValue].ChipTop;
                        inputOption = new ChipInputOptionReference(referenceChip);
                    }
                    else if (jsonInputData.InputType.Equals("Value"))
                    {
                        object value = null;
                        var inputTypeName = chip.ChipData.GetInputType(i + 1);
                        if(inputTypeName.Equals("Template"))
                        {
                            value = Templates.BlockTemplates[jsonInputData.InputValue];
                        }
                        else
                        {
                            if (inputTypeName.Equals("int")) { inputTypeName = "Int32"; }
                            var type = TypeUtils.AllTypes[inputTypeName];
                            value = TypeUtils.ParseType(type, jsonInputData.InputValue);
                        }

                        inputOption = new ChipInputOptionValue(value);
                    }

                    chipTop.ManuallySetInputSection(inputOption, i);
                }

                if (chip.ChipData.ChipDataType == ChipType.Control)
                {
                    _setControlChipTargets(chip, blocksDict);
                }
            }

            var baseChipset = chipsetsJson.First(c => c.Name.Equals("_Initial")).Chipset;
            return baseChipset;
        }

        private static void _setControlChipTargets(ChipJSONData chip, Dictionary<string, ChipBlockJSONData> chipsets)
        {
            var data = chip.ChipData;

            if (data.Name.Equals("If"))
            {
                var ifChip = (ChipTopSwitch)chip.ChipTop;

                var yesBlock = chipsets[chip.Yes].Chipset;
                ifChip.AddSwitchSection("Yes", yesBlock);

                var noBlock = chipsets[chip.No].Chipset;
                ifChip.AddSwitchSection("No", noBlock);
            }
            if (data.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (ChipTopSwitch)chip.ChipTop;
                foreach (var keyEffect in chip.KeyEffects)
                {
                    var keyString = keyEffect.Item1;
                    var blockName = keyEffect.Item2;
                    var block = chipsets[blockName].Chipset;
                    keySwitchChip.AddSwitchSection(keyString, block);
                }
            }
        }
        #endregion
    }
}
