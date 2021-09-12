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

            if(inputType.Equals(InGameTypeUtils.Variable))
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
            if (BlockDataUtils.IsIfBlock(BlockData))
            {
                var ifChip = (IIfChip)Chip;

                Yes = ifChip.Yes.Name;
                No = ifChip.No.Name;
            }
            else if (BlockData.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (KeySwitchChip)Chip;
                KeyEffects = keySwitchChip.KeyEffectsToString();
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
