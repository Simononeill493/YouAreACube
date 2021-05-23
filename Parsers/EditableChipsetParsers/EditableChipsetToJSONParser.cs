using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class EditableChipsetToJSONParser
    {
        public static GraphicalChipData _getFirstMatchingMapping(List<string> inputs, List<GraphicalChipData> possibleMatches)
        {
            foreach (var possibleMatch in possibleMatches)
            {
                for (int i = 0; i < inputs.Count; i++)
                {
                    if (!TemplateEditUtils.IsValidInputFor(inputs[i],possibleMatch.Inputs[i]))
                    {
                        goto End;
                    }
                }

                return possibleMatch;
                End:;
            }

            return null;
        }



        public static string ParseEditableChipsetToJson(EditableChipset chipset)
        {
            var chipsetsJson = new ChipsetJSONData();

            var thisAndSubs = chipset.GetThisAndSubChipsets();
            foreach (var editableChipset in chipset.GetThisAndSubChipsets())
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
                    chipJObject.GraphicalChipType = chip.ChipData.BaseMappingName;
                    chipJObject.ActualChipType = chip.ChipData.Name;

                    chipJObject.Name = chip.Name;

                    GraphicalChipData mappedChipType = null;

                    if (chip.ChipData.IsMappedToSubChips)
                    {
                        var selectedTypes = chip.GetSelectedInputTypes();
                        var mappings = chip.ChipData.InputMappings;
                        var mappedType = _getFirstMatchingMapping(selectedTypes, chip.ChipData.InputMappings);

                        mappedChipType = mappedType;
                        chipJObject.ActualChipType = mappedType.Name;
                    }

                    if (chip.CurrentTypeArguments.Count>0 & chip.CurrentTypeArguments.First().Length>0 & mappedChipType == null)
                    {
                        chipJObject.TypeArguments = chip.CurrentTypeArguments;
                    }

                    if(mappedChipType != null)
                    {
                        if(mappedChipType.IsGeneric)
                        {
                            chipJObject.TypeArguments = chip.CurrentTypeArguments;
                        }
                    }

                    chipJObject.Inputs = new List<ChipJSONInputData>();
                    var inputsList = chip.GetCurrentInputs();
                    for(int i=0;i<inputsList.Count;i++)
                    {
                        var input = inputsList[i];
                        var inputOptionType = input.OptionType.ToString();
                        if(inputOptionType.Equals(nameof(InputOptionType.Parseable)))
                        {
                            inputOptionType = nameof(InputOptionType.Value);
                        }

                        var inputData = new ChipJSONInputData(inputOptionType, input.ToString());
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
                chipJObject.Yes = switchChip.SwitchChipsets[0].Name;
                chipJObject.No = switchChip.SwitchChipsets[1].Name;

            }
            if (chipJObject.ChipData.Name.Equals("KeySwitch"))
            {
                chipJObject.KeyEffects = new List<Tuple<string, string>>();

                foreach (var keyAndChipset in switchChip.GetSwitchSectionsWithNames())
                {
                    chipJObject.KeyEffects.Add(new Tuple<string, string>(keyAndChipset.Item1, keyAndChipset.Item2.Name));
                }
            }
        }
    }
}
