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
            IChip = iChip;
            GraphicalChipData = iChip.GetChipData();

            Name = iChip.Name;
            GraphicalChipType = GraphicalChipData.BaseMappingName;
            ActualChipType = GraphicalChipData.Name;

            CreateInputsBlank();
        }

        public ChipJSONData(ChipTop chip)
        {
            ChipTop = chip;
            GraphicalChipData = chip.ChipData;

            Name = chip.Name;
            GraphicalChipType = chip.ChipData.BaseMappingName;
            ActualChipType = chip.ChipData.Name;
        }


        [JsonIgnore]
        public GraphicalChipData GraphicalChipData;
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
        public ChipTop ChipTop;
        public void CreateChipTop(IChipsetGenerator generator)
        {
            var dataToCreateWith = GraphicalChipData;
            if(GraphicalChipData.BaseMappingChip!=null)
            {
                dataToCreateWith = GraphicalChipData.BaseMappingChip;
            }

            ChipTop = ChipTop.GenerateChipFromChipData(dataToCreateWith, this.Name);
            ChipTop.SetGenerator(generator);
        }

        [JsonIgnore]
        public IChip IChip;
        public void CreateIChip()
        {
            IChip = ChipObjectGenerator.GenerateIChipFromChipData(GraphicalChipData, TypeArguments);
            IChip.Name = this.Name;
        }



        public string Yes;
        public string No;
        public List<(string keyString, string blockName)> KeyEffects;
    }
}
