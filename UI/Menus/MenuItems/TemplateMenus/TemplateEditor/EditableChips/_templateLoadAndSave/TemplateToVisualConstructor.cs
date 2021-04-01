using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateToVisualConstructor
    { 
        public static void AddTemplateChipsToChipset(EditableChipset topLevelChipset,BlockTemplate template, IChipsetGenerator generator)
        {
            if (!template.Active)
            {
                return;
            }

            var chipBlock = template.Chips;
            var chipsFromThisChipset = new List<ChipTop>();

            foreach(var chip in chipBlock.Chips)
            {
                var chipData = ChipDatabase.GetChipDataFromChip(chip);
                var chipTop = GenerateChipFromChipData(chipData);
                chipTop.GenerateSubChipsets(generator);
                chipsFromThisChipset.Add(chipTop);
            }

            topLevelChipset.AppendChips(chipsFromThisChipset,0);
        }

        public static ChipTop GenerateChipFromChipData(ChipData data)
        {
            var initialDrawLayer = ManualDrawLayer.Zero;

            if (data.Name.Equals("If"))
            {
                return new ChipTopSwitch(initialDrawLayer, data, new List<string>() { "Yes", "No" });
            }
            else
            {
                return new ChipTopStandard(initialDrawLayer, data);
            }
        }
    }
}
