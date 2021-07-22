using System.Collections.Generic;
using Newtonsoft.Json;

namespace IAmACube
{
    public class ChipBlockJSONData
    {
        public string Name;
        public List<ChipJSONData> Chips;

        [JsonIgnore]
        public Blockset Blockset;
        public void CreateChipset(IChipsetGenerator generator)
        {
            var chipset = generator.CreateChipset(Name);
            chipset.TopLevelRefreshAll = () => { };

            Blockset = chipset;
        }

        [JsonIgnore]
        public Chipset ChipBlock;
        public void CreateChipBlock()
        {
            ChipBlock = new Chipset() { Name = this.Name };
        }

        public ChipBlockJSONData() { }

        public ChipBlockJSONData(Blockset chipset)
        {
            Chips = new List<ChipJSONData>();
            Blockset = chipset;
            Name = chipset.Name;
        }

        public ChipBlockJSONData(Chipset block)
        {
            ChipBlock = block;
            Name = block.Name;
        }
    }
}
