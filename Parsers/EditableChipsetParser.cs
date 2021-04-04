using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class EditableChipsetParser
    { 
        public static EditableChipset ParseJsonToEditableChipset(string json,IChipsetGenerator generator)
        {
            var chipBlocksToken = JToken.Parse(json);
            var chipsets = new Dictionary<string, EditableChipset>();
            var chips = new Dictionary<string, ChipTop>();
            var chipToObject = new Dictionary<string, JToken>();

            foreach (var blockToken in chipBlocksToken)
            {
                var blockName = blockToken["Name"].ToString();
                var chipset = generator.CreateChipset();
                chipset.TopLevelRefreshAll = () => { };

                chipset.Name = blockName;
                chipsets[blockName] = chipset;

                var chipsTokens = blockToken["Chips"];
                foreach(var chipToken in chipsTokens)
                {
                    var chipName = chipToken["Name"].ToString();
                    var chipDataName = chipToken["Type"].ToString();
                    var chipData = ChipDatabase.BuiltInChips[chipDataName];
                    var chipTop = ChipTop.GenerateChipFromChipData(chipData);
                    chipTop.SetGenerator(generator);
                    
                    chips[chipName] = chipTop;
                    chipToObject[chipName] = chipToken;

                    chipset.AppendChipToEnd(chipTop);
                }
            }

            foreach(var chip in chips)
            {
                var data = chip.Value.ChipData;
                if (data.ChipDataType == ChipType.Control)
                {
                    _setControlChipData(chip.Value, data, chipToObject[chip.Key], chipsets);
                }
            }

            var baseChipset = chipsets.Values.First(c=>c.Name.Equals("Initial"));
            return baseChipset;
        }

        private static void _setControlChipData(ChipTop chip,ChipData data, JToken chipToken, Dictionary<string, EditableChipset> chipsets)
        {
            if(data.Name.Equals("If"))
            {
                var ifChip = (ChipTopSwitch)chip;

                var yesName = chipToken["Yes"].ToString();
                var yesBlock = chipsets[yesName];
                ifChip.AddSwitchSection("Yes", yesBlock);

                var noName = chipToken["No"].ToString();
                var noBlock = chipsets[noName];
                ifChip.AddSwitchSection("No", noBlock);
            }
        }

        public static string ParseEditableChipsetToJson(EditableChipset chipset)
        {




            return null;
        }
    }
}
