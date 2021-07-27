using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace IAmACube
{
    public class ChipJSONData
    {
        public string Name;
        public string GraphicalChipType;
        public string ActualChipType;
        public List<string> TypeArguments;

        public List<ChipJSONInputData> Inputs;
        public object ParseInput(int inputIndex) => Inputs[inputIndex].Parse(BlockData.GetInputType(inputIndex));

        public ChipJSONData() { }

        public ChipJSONData(IChip iChip) 
        {
            Chip = iChip;
            BlockData = iChip.GetBlockData();

            Name = iChip.Name;
            GraphicalChipType = BlockData.BaseMappingName;
            ActualChipType = BlockData.Name;

            _setBlankInputs();
            _setTypeArgumentsFromChip();
            _setSubChipsetsFromChip();
        }
        private void _setBlankInputs()
        {
            Inputs = new List<ChipJSONInputData>();
            for (int i = 0; i < BlockData.NumInputs; i++)
            {
                Inputs.Add(new ChipJSONInputData(InputOptionType.Undefined, ""));
            }
        }
        private void _setTypeArgumentsFromChip()
        {
            if (BlockData.IsGeneric)
            {
                var type = Chip.GetType();
                var typeArguments = type.GenericTypeArguments;
                TypeArguments = typeArguments.Select(ta => TypeUtils.GetTypeDisplayName(ta)).ToList();
            }
        }
        private void _setSubChipsetsFromChip()
        {
            if (BlockData.Name.Equals("If"))
            {
                var ifChip = (IfChip)Chip;

                Yes = ifChip.Yes.Name;
                No = ifChip.No.Name;
            }
            else if (BlockData.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (KeySwitchChip)Chip;
                var keysAndEffects = new List<(string, string)>();

                foreach (var keyBlock in keySwitchChip.KeyEffects)
                {
                    keysAndEffects.Add((keyBlock.Key.ToString(), keyBlock.Chipset.Name));
                }

                KeyEffects = keysAndEffects;
            }
        }



        public ChipJSONData(BlockTop block)
        {
            Block = block;
            BlockData = block.BlockData;

            Name = block.Name;
            GraphicalChipType = block.BlockData.BaseMappingName;

            _setBlockSubMapping();
            _setTypeArgumentsFromBlock();
            _setSelectedInputsFromBlock();
            _setControlBlockTargets();
        }
        private void _setBlockSubMapping()
        {
            var selectedTypes = Block.GetSelectedInputTypes();
            MappedBlockData =  BlockData.GetMappedBlockData(selectedTypes);
            ActualChipType = MappedBlockData.Name;
        }
        private void _setTypeArgumentsFromBlock()
        {
            if (MappedBlockData.IsGeneric)
            {
                TypeArguments = Block.CurrentTypeArguments;
            }
        }
        private void _setSelectedInputsFromBlock()
        {
            Inputs = new List<ChipJSONInputData>();
            var inputsList = Block.GetCurrentInputs();
            for (int i = 0; i < inputsList.Count; i++)
            {
                _addBlockInputOption(inputsList[i]);
            }
        }
        private void _addBlockInputOption(BlockInputOption blockInputOption)
        {
            var inputOptionType = blockInputOption.OptionType;
            if (inputOptionType.Equals(InputOptionType.Parseable))
            {
                inputOptionType = InputOptionType.Value;
            }

            var inputData = new ChipJSONInputData(inputOptionType, blockInputOption.ToString());
            Inputs.Add(inputData);
        }
        private void _setControlBlockTargets()
        {
            if (BlockData.Name.Equals("If"))
            {
                var ifChip = (BlockTopSwitch)Block;

                Yes = ifChip.SwitchBlocksets[0].Name;
                No = ifChip.SwitchBlocksets[1].Name;

            }
            if (BlockData.Name.Equals("KeySwitch"))
            {
                var keyChip = (BlockTopSwitch)Block;

                KeyEffects = new List<(string, string)>();

                foreach (var keyAndChipset in keyChip.GetSwitchSectionsWithNames())
                {
                    KeyEffects.Add((keyAndChipset.Item1, keyAndChipset.Item2.Name));
                }
            }
        }






        [JsonIgnore]
        public BlockData MappedBlockData;

        [JsonIgnore]
        public BlockData BlockData;
        public void SetChipData()
        {
            if(ActualChipType != null)
            {
                BlockData = BlockDataDatabase.BlockDataDict[ActualChipType];
            }
            else
            {
                BlockData = BlockDataDatabase.BlockDataDict[GraphicalChipType];
            }
        }

        [JsonIgnore]
        public BlockTop Block;
        public void CreateChipTop(IBlocksetGenerator generator)
        {
            var dataToCreateWith = BlockData;
            if(BlockData.BaseMappingBlock!=null)
            {
                dataToCreateWith = BlockData.BaseMappingBlock;
            }

            Block = BlockTop.GenerateChipFromChipData(dataToCreateWith, this.Name);
            Block.SetGenerator(generator);
        }

        [JsonIgnore]
        public IChip Chip;
        public void CreateChip()
        {
            Chip = BlockData.GenerateChip(TypeArguments);
            Chip.Name = this.Name;
        }

        public string Yes;
        public string No;
        public List<(string keyString, string blockName)> KeyEffects;

    }
}
