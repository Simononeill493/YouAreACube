using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipBlockParser
    {
        public static string ParseBlockIntoJson(ChipBlock toParse)
        {
            JObject outputObject = new JObject();
            var allChips = toParse.GetAllChipsAndSubChips();

            foreach(var chip in toParse.Chips)
            {
                var chipData = ChipDatabase.GetChipDataFromChip(chip);
                var chipType = chip.GetType();
                if(chipData.HasOutput)
                {
                    var targets1 = chipType.GetField("Targets").GetValue(chip);//.Where(p => p.Name.Equals("Targets"));
                    var targets2 = chipType.GetField("Targets2").GetValue(chip);//.Where(p => p.Name.Equals("Targets"));
                    var targets3 = chipType.GetField("Targets3").GetValue(chip);//.Where(p => p.Name.Equals("Targets"));

                }
            }
            return outputObject.ToString();
        }

        private static Type _getOutputPinTypeMetadata(Type t)
        {
            if(t.Name.Contains("Output"))
            {
                return t;
            }

            if(t.BaseType == null)
            {
                return null;
            }
            else
            {
                return _getOutputPinTypeMetadata(t.BaseType);
            }
        }
    }
}
