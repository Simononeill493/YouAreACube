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
        public void CreateInputsBlank()
        {
            Inputs = new List<ChipJSONInputData>();
            for (int i = 0; i < BlockData.NumInputs; i++)
            {
                Inputs.Add(new ChipJSONInputData("",""));
            }
        }
        public object ParseInput(int inputIndex) => Inputs[inputIndex].Parse(BlockData.GetInputType(inputIndex));

        public ChipJSONData() { }

        public ChipJSONData(IChip iChip) 
        {
            Chip = iChip;
            BlockData = iChip.GetChipData();

            Name = iChip.Name;
            GraphicalChipType = BlockData.BaseMappingName;
            ActualChipType = BlockData.Name;

            CreateInputsBlank();

            _setTypeArgumentsFromChip();
            _setSubChipsetsFromChip();
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



        public ChipJSONData(BlockTop chip)
        {
            Block = chip;
            BlockData = chip.BlockData;

            Name = chip.Name;
            GraphicalChipType = chip.BlockData.BaseMappingName;
            ActualChipType = chip.BlockData.Name;
        }


        [JsonIgnore]
        public BlockData BlockData;
        public void SetChipData()
        {
            if(ActualChipType != null)
            {
                BlockData = BlockDataDatabase.GraphicalChips[ActualChipType];
            }
            else
            {
                BlockData = BlockDataDatabase.GraphicalChips[GraphicalChipType];
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
