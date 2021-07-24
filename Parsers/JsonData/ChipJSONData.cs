using System;
using System.Collections.Generic;
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
            for (int i = 0; i < GraphicalChipData.NumInputs; i++)
            {
                Inputs.Add(new ChipJSONInputData("",""));
            }
        }
        public object ParseInput(int inputIndex) => Inputs[inputIndex].Parse(GraphicalChipData.GetInputType(inputIndex));

        public ChipJSONData() { }

        public ChipJSONData(IChip iChip) 
        {
            Chip = iChip;
            GraphicalChipData = iChip.GetChipData();

            Name = iChip.Name;
            GraphicalChipType = GraphicalChipData.BaseMappingName;
            ActualChipType = GraphicalChipData.Name;

            CreateInputsBlank();
        }

        public ChipJSONData(BlockTop chip)
        {
            Block = chip;
            GraphicalChipData = chip.BlockData;

            Name = chip.Name;
            GraphicalChipType = chip.BlockData.BaseMappingName;
            ActualChipType = chip.BlockData.Name;
        }


        [JsonIgnore]
        public BlockData GraphicalChipData;
        public void SetChipData()
        {
            if(ActualChipType != null)
            {
                GraphicalChipData = GraphicalChipDatabase.GraphicalChips[ActualChipType];
            }
            else
            {
                GraphicalChipData = GraphicalChipDatabase.GraphicalChips[GraphicalChipType];
            }
        }

        [JsonIgnore]
        public BlockTop Block;
        public void CreateChipTop(IBlocksetGenerator generator)
        {
            var dataToCreateWith = GraphicalChipData;
            if(GraphicalChipData.BaseMappingBlock!=null)
            {
                dataToCreateWith = GraphicalChipData.BaseMappingBlock;
            }

            Block = BlockTop.GenerateChipFromChipData(dataToCreateWith, this.Name);
            Block.SetGenerator(generator);
        }

        [JsonIgnore]
        public IChip Chip;
        public void CreateChip()
        {
            Chip = ChipObjectGenerator.GenerateIChipFromChipData(GraphicalChipData, TypeArguments);
            Chip.Name = this.Name;
        }



        public string Yes;
        public string No;
        public List<(string keyString, string blockName)> KeyEffects;
    }
}
