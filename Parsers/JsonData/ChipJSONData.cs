using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IAmACube
{
    public class ChipJSONData
    {
        public string Name;
        public string Type;
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
        public ChipData ChipData;
        public void SetChipData() => ChipData = ChipDatabase.BuiltInChips[Type];

        [JsonIgnore]
        public ChipTop ChipTop;
        public void CreateChipTop(IChipsetGenerator generator)
        {
            ChipTop = ChipTop.GenerateChipFromChipData(ChipData, this.Name);
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
