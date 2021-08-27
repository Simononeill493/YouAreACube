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
        public object ParseInput(int inputIndex)
        {
            var inputData = Inputs[inputIndex];
            var inputType = BlockData.GetInputType(inputIndex);

            if(inputType.Equals("Variable"))
            {
                inputType = TypeArguments.First();
            }

            return inputData.Parse(inputType);
        }
        

        public ChipJSONData() { }

        public ChipJSONData(IChip iChip) 
        {
            Chip = iChip;
            BlockData = iChip.GetBlockData();
            Name = iChip.Name;
            GraphicalChipType = BlockData.BaseMappingName;
            ActualChipType = BlockData.Name;

            if (BlockData.IsGeneric)
            {
                TypeArguments = Chip.GetTypeArgumentNames();
            }

            _setControlChipTargets();
        }
        private void _setControlChipTargets()
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
                KeyEffects = keySwitchChip.KeyEffectsToString();
            }
        }



        public ChipJSONData(BlockTop block)
        {
            Block = block;
            BlockData = block.BlockData;
            Name = block.Name;
            GraphicalChipType = block.BlockData.BaseMappingName;
            MappedBlockData = BlockData.GetMappedBlockData(Block.GetSelectedInputTypes());
            ActualChipType = MappedBlockData.Name;

            if (MappedBlockData.IsGeneric)
            {
                TypeArguments = Block.CurrentTypeArguments;
            }

            Inputs = ChipJSONInputData.GenerateInputsFromBlock(Block);
            _setControlBlockTargets();
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
        public void SetBlockData()
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
        public void CreateBlockTop(IBlocksetTopLevelContainer container)
        {
            var dataToCreateWith = BlockData;
            if(BlockData.BaseMappingBlock!=null)
            {
                dataToCreateWith = BlockData.BaseMappingBlock;
            }

            Block = BlockUtils.GenerateBlockFromBlockData(dataToCreateWith, this.Name);
            Block.SetInitialTypeArguments(TypeArguments);
            Block.TopLevelContainer = container;
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
