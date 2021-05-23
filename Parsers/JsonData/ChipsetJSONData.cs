using System;
using System.Collections.Generic;
using System.Linq;

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

        public void CreateEditableChipsetObjects(IChipsetGenerator generator)
        {
            CreateChipsets(generator);
            CreateChipTops(generator);
        }
        public void CreateChipsets(IChipsetGenerator generator) => this.ForEach(c => c.CreateChipset(generator));
        public void CreateChipTops(IChipsetGenerator generator) => GetChips().ForEach(c => c.CreateChipTop(generator));
        
        public void CreateChipBlockObjects()
        {
            CreateChipBlocks();
            CreateIChips();
        }
        public void CreateChipBlocks() => this.ForEach(c => c.CreateChipBlock());
        public void CreateIChips() => GetChips().ForEach(c => c.CreateIChip());

        public ChipBlockJSONData GetInitial()
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
    }
}
