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

        public ChipsetJSONData(Blockset blockset)
        {
            Chips = new List<ChipJSONData>();
            Blockset = blockset;
            Name = blockset.Name;
        }

        public ChipsetJSONData(Chipset chipset)
        {
            Chipset = chipset;
            Name = chipset.Name;
        }
    }
}
