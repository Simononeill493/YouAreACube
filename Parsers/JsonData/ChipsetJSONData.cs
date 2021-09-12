using System.Collections.Generic;
using Newtonsoft.Json;

namespace IAmACube
{
    public class ChipsetJSONData
    {
        public string Name;
        public List<ChipJSONData> Chips;

        [JsonIgnore]
        public Chipset Chipset;
        public void CreateChipset()
        {
            Chipset = new Chipset() { Name = this.Name };
        }

        public ChipsetJSONData() { }

        public ChipsetJSONData(Chipset chipset)
        {
            Chipset = chipset;
            Name = chipset.Name;
        }
    }
}
