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
        public string TypeArgument;

        public List<ChipJSONInputData> Inputs;
        public void SetInputs()
        {
            Inputs = new List<ChipJSONInputData>();
            for (int i = 0; i < ChipData.NumInputs; i++)
            {
                Inputs.Add(new ChipJSONInputData("",""));
            }
        }

        [JsonIgnore]
        public GraphicalChipData ChipData;
        public void SetChipData()
        {
            if(ActualChipType != null)
            {
                ChipData = GraphicalChipDatabase.GraphicalChips[ActualChipType];
            }
            else
            {
                ChipData = GraphicalChipDatabase.GraphicalChips[GraphicalChipType];
            }
        }

        [JsonIgnore]
        public ChipTop ChipTop;
        public void CreateChipTop(IChipsetGenerator generator)
        {
            var dataToCreateWith = ChipData;
            if(ChipData.BaseMappingChip!=null)
            {
                dataToCreateWith = ChipData.BaseMappingChip;
            }

            ChipTop = ChipTop.GenerateChipFromChipData(dataToCreateWith, this.Name);
            ChipTop.SetGenerator(generator);
        }

        [JsonIgnore]
        public IChip IChip;
        public void CreateIChip()
        {
            IChip = ChipObjectGenerator.GenerateIChipFromChipData(ChipData, TypeArgument);
            IChip.Name = this.Name;
        }

        public string Yes;
        public string No;
        public List<Tuple<string, string>> KeyEffects;
    }
}
