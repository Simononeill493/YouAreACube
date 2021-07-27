using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class Parser_BlocksetToJSON
    {
        public static string ParseBlocksetToJson(Blockset chipset)
        {
            var fullJSON = new FullChipsetJSONData();

            foreach (var blockset in chipset.GetThisAndSubChipsets())
            {
                var chipsetJSON = new ChipsetJSONData(blockset);
                _addBlocksToChipsetJSON(chipsetJSON, blockset.Blocks);

                fullJSON.Add(chipsetJSON);
            }

            fullJSON.AlphabetSort();
            return fullJSON.GenerateString();
        }

        private static void _addBlocksToChipsetJSON(ChipsetJSONData chipsetJSON, List<BlockTop> blocks)
        {
            foreach (var block in blocks)
            {
                var chipJSON = _parseBlock(block);
                chipsetJSON.Chips.Add(chipJSON);
            }
        }

        private static ChipJSONData _parseBlock(BlockTop block)
        {
            var chipJSON = new ChipJSONData(block);

            var mappedBlockType = _getBlockSubMapping(chipJSON);
            _setBlockTypeArguments(chipJSON, mappedBlockType);
            _parseSelectedBlockInputs(chipJSON);
            _setControlBlockTargets(chipJSON);

            return chipJSON;
        }

        public static BlockData _getBlockSubMapping(ChipJSONData chipJObject)
        {
            if (chipJObject.BlockData.IsMappedToSubChips)
            {
                var selectedTypes = chipJObject.Block.GetSelectedInputTypes();
                var mappedChipType = _getFirstMatchingMapping(selectedTypes, chipJObject.BlockData.InputMappings);

                chipJObject.ActualChipType = mappedChipType.Name;
                return mappedChipType;
            }

            return null;
        }

        public static void _setBlockTypeArguments(ChipJSONData chipJObject, BlockData mappedChipType)
        {
            var chip = chipJObject.Block;

            if (chip.CurrentTypeArguments.Count > 0 & chip.CurrentTypeArguments.First().Length > 0 & mappedChipType == null)
            {
                chipJObject.TypeArguments = chip.CurrentTypeArguments;
            }

            if (mappedChipType != null)
            {
                if (mappedChipType.IsGeneric)
                {
                    chipJObject.TypeArguments = chip.CurrentTypeArguments;
                }
            }
        }

        public static void _parseSelectedBlockInputs(ChipJSONData chipJObject)
        {
            chipJObject.Inputs = new List<ChipJSONInputData>();
            var inputsList = chipJObject.Block.GetCurrentInputs();
            for (int i = 0; i < inputsList.Count; i++)
            {
                var input = inputsList[i];
                var inputOptionType = input.OptionType;
                if (inputOptionType.Equals(InputOptionType.Parseable))
                {
                    inputOptionType = InputOptionType.Value;
                }

                var inputData = new ChipJSONInputData(inputOptionType, input.ToString());
                chipJObject.Inputs.Add(inputData);
            }
        }

        private static void _setControlBlockTargets(ChipJSONData chipJObject)
        {
            if(chipJObject.BlockData.Name.Equals("If"))
            {
                var ifChip = (BlockTopSwitch)chipJObject.Block;

                chipJObject.Yes = ifChip.SwitchBlocksets[0].Name;
                chipJObject.No = ifChip.SwitchBlocksets[1].Name;

            }
            if (chipJObject.BlockData.Name.Equals("KeySwitch"))
            {
                var keyChip = (BlockTopSwitch)chipJObject.Block;

                chipJObject.KeyEffects = new List<(string, string)>();

                foreach (var keyAndChipset in keyChip.GetSwitchSectionsWithNames())
                {
                    chipJObject.KeyEffects.Add((keyAndChipset.Item1, keyAndChipset.Item2.Name));
                }
            }
        }

        public static BlockData _getFirstMatchingMapping(List<string> inputs, List<BlockData> possibleMatches)
        {
            foreach (var possibleMatch in possibleMatches)
            {
                if(_doesMappingMatchInputs(inputs,possibleMatch))
                {
                    return possibleMatch;
                }
            }

            return null;
        }
        public static bool _doesMappingMatchInputs(List<string> inputs,BlockData possibleMatch)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                if (!TemplateEditUtils.IsValidInputFor(inputs[i], possibleMatch.Inputs[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
