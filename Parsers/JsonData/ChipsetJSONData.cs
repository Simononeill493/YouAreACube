using System.Collections.Generic;

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
}
