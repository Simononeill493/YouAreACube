using System.Collections.Generic;
using Newtonsoft.Json;

namespace IAmACube
{
    public class ChipBlockJSONData
    {
        public string Name;
        public List<ChipJSONData> Chips;

        [JsonIgnore]
        public EditableChipset Chipset;
        public void CreateChipset(IChipsetGenerator generator)
        {
            var chipset = generator.CreateChipset(Name);
            chipset.TopLevelRefreshAll = () => { };

            Chipset = chipset;
        }

        [JsonIgnore]
        public ChipBlock ChipBlock;
        public void CreateChipBlock()
        {
            ChipBlock = new ChipBlock() { Name = this.Name };
        }
    }
}
