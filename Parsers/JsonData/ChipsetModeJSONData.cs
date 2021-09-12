using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public class ChipsetModeJSONData : List<ChipsetJSONData>
    {
        public ChipsetModeJSONData() { }
        public ChipsetModeJSONData(Chipset chipsetToAdd)
        {
            ChipsDict = chipsetToAdd.MakeChipsDict();

            foreach (var chipset in chipsetToAdd.GetThisAndAllChipsetsCascade())
            {
                var chipsetJSON = new ChipsetJSONData(chipset);
                chipsetJSON.Chips = ChipsDict.FetchJSON(chipset.Chips);
                this.Add(chipsetJSON);
            }
        }


        [JsonIgnore]
        public Dictionary<string, ChipsetJSONData> ChipsetsDict;
        [JsonIgnore]
        public Dictionary<string, ChipJSONData> ChipsDict;

        public void MakeDicts()
        {
            ChipsetsDict = this.ToDict();
            ChipsDict = GetChips().ToDict();
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
       
        public void SetBlockData() => GetChips().ForEach(c => c.SetBlockData());

        public void CreateChipsetObjects()
        {
            CreateChipsets();
            CreateChips();
        }
        public void CreateChipsets() => this.ForEach(c => c.CreateChipset());
        public void CreateChips() => GetChips().ForEach(c => c.CreateChip());
        public void AppendChipsToChipsets()
        {
            foreach (var chipsetJSON in this)
            {
                foreach (var chipJSON in chipsetJSON.Chips)
                {
                    chipsetJSON.Chipset.AddChip(chipJSON.Chip);
                }
            }
        }

        public ChipsetJSONData GetInitial()
        {
            var init = this.Where(c => c.Name.Equals(ChipsetCollection.InitialChipName));
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

