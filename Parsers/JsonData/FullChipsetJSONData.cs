using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public class FullChipsetJSONData : List<ChipsetJSONData>
    {
        public Dictionary<string, ChipsetJSONData> GetChipsetsDict()
        {
            var output = new Dictionary<string, ChipsetJSONData>();
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
            foreach (var chipsetJSON in this)
            {
                output.AddRange(chipsetJSON.Chips);
            }
            return output;
        }
    
        public void SetChipData() => GetChips().ForEach(c => c.SetChipData());

        public void CreateEditableChipsetObjects(IBlocksetGenerator generator)
        {
            CreateBlocksets(generator);
            CreateBlocksetChipTops(generator);
        }
        public void CreateBlocksets(IBlocksetGenerator generator) => this.ForEach(c => c.CreateBlockset(generator));
        public void CreateBlocksetChipTops(IBlocksetGenerator generator) => GetChips().ForEach(c => c.CreateChipTop(generator));
        
        public void CreateChipsetObjects()
        {
            CreateChipsets();
            CreateChips();
        }
        public void CreateChipsets() => this.ForEach(c => c.CreateChipset());
        public void CreateChips() => GetChips().ForEach(c => c.CreateChip());

        public ChipsetJSONData GetInitial()
        {
            var init = this.Where(c => c.Name.Equals("_Initial"));
            if(init.Count()>1)
            {
                throw new Exception("Multiple initial chip blocks");
            }
            if (init.Count() < 1)
            {
                throw new Exception("No initial chip block");
            }

            return init.First();
        }

        public void AlphabetSort() => Sort((c1, c2) => string.Compare(c1.Name, c2.Name));
        public string GenerateString() => JToken.FromObject(this, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore }).ToString();
    }
}

