﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class JSONToEditableChipsetParser
    {
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
                    var inputType = chip.ChipData.GetInputType(i);

                    ChipInputOption inputOption = null;
                    if (jsonInputData.InputType.Equals("Reference"))
                    {
                        var referenceChip = (ChipTopWithOutput)chipsDict[jsonInputData.InputValue].ChipTop;
                        inputOption = new ChipInputOptionReference(referenceChip);
                    }
                    else if (jsonInputData.InputType.Equals("Value"))
                    {
                        object value = null;
                        var inputTypeName = chip.ChipData.GetInputType(i);
                        if (inputTypeName.Equals("Template"))
                        {
                            value = Templates.Database[jsonInputData.InputValue];
                        }
                        else
                        {
                            var type = TypeUtils.GetTypeByName(inputTypeName);
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
    }
}