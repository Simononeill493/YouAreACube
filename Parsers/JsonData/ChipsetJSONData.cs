using System.Collections.Generic;
using Newtonsoft.Json;

namespace IAmACube
{
    public class ChipsetJSONData
    {
        public string Name;
        public List<ChipJSONData> Chips;

        [JsonIgnore]
        public Blockset Blockset;
        public void CreateBlockset(IBlocksetGenerator generator)
        {
            var chipset = generator.CreateBlockset(Name);
            chipset.TopLevelRefreshAll = () => { };

            Blockset = chipset;
        }

        [JsonIgnore]
        public Chipset Chipset;
        public void CreateChipset()
        {
            Chipset = new Chipset() { Name = this.Name };
        }

        public ChipsetJSONData() { }

        public ChipsetJSONData(Blockset chipset)
        {
            Chips = new List<ChipJSONData>();
            Blockset = chipset;
            Name = chipset.Name;
        }

        public ChipsetJSONData(Chipset block)
        {
            Chipset = block;
            Name = block.Name;
        }
    }
}
