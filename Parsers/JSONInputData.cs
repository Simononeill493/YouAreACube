using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IAmACube
{
    public class ChipsetJSONData : List<ChipBlockJSONData>
    {
        public Dictionary<string, ChipBlockJSONData> GetBlocksDict()
        {
            var output = new Dictionary<string, ChipBlockJSONData>();
            foreach (var block in this)
            {
                output[block.Name] = block;
            }
            return output;
        }
        public Dictionary<string, ChipJSONData> GetChipsDict()
        {
            var output = new Dictionary<string, ChipJSONData>();
            foreach(var chip in GetChips())
            {
                output[chip.Name] = chip;

            }
            return output;
        }
        public List<ChipJSONData> GetChips()
        {
            var output = new List<ChipJSONData>();
            foreach (var block in this)
            {
                output.AddRange(block.Chips);
            }
            return output;
        }
    
        public void SetChipData() => GetChips().ForEach(c => c.SetChipData());

        public void CreateChipsets(IChipsetGenerator generator) => this.ForEach(c => c.CreateChipset(generator));
        public void CreateChipTops(IChipsetGenerator generator) => GetChips().ForEach(c => c.CreateChipTop(generator));

        public void CreateChipBlocks() => this.ForEach(c => c.CreateChipBlock());
        public void CreateIChips() => GetChips().ForEach(c => c.CreateIChip());
    }

    public class ChipBlockJSONData
    {
        public string Name;
        public List<ChipJSONData> Chips;

        [JsonIgnore]
        public EditableChipset Chipset;
        public void CreateChipset(IChipsetGenerator generator)
        {
            var chipset = generator.CreateChipset();
            chipset.TopLevelRefreshAll = () => { };
            chipset.Name = Name;

            Chipset = chipset;
        }

        [JsonIgnore]
        public ChipBlock ChipBlock;
        public void CreateChipBlock()
        {
            ChipBlock = new ChipBlock() { Name = this.Name };
        }
    }

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
                Inputs.Add(new ChipJSONInputData() { InputType = "", InputValue = "" });
            }
        }

        [JsonIgnore]
        public ChipData ChipData;
        public void SetChipData() => ChipData = ChipDatabase.BuiltInChips[Type];

        [JsonIgnore]
        public ChipTop ChipTop;
        public void CreateChipTop(IChipsetGenerator generator)
        {
            ChipTop = ChipTop.GenerateChipFromChipData(ChipData);
            ChipTop.SetGenerator(generator);

            ChipTop.Name = this.Name;
        }

        [JsonIgnore]
        public IChip IChip;
        public void CreateIChip()
        {
            if (ChipData.IsGeneric)
            {
                IChip = ChipDatabase.GenerateChipFromData(ChipData, TypeArgument);
            }
            else
            {
                IChip = ChipDatabase.GenerateChipFromData(ChipData);
            }

            IChip.Name = this.Name;
        }

        public string Yes;
        public string No;
        public List<Tuple<string, string>> KeyEffects;
    }

    public class ChipJSONInputData
    {
        public string InputType;
        public string InputValue;
    }
}
