using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class JSONDataUtils
    {
        public static Dictionary<string, ChipJSONData> ToDict(this IEnumerable<ChipJSONData> chips)
        {
            var output = new Dictionary<string, ChipJSONData>();
            foreach (var chip in chips)
            {
                output[chip.Name] = chip;

            }
            return output;
        }

        public static Dictionary<string, ChipsetJSONData> ToDict(this IEnumerable<ChipsetJSONData> chipsets)
        {
            var output = new Dictionary<string, ChipsetJSONData>();
            foreach (var chipset in chipsets)
            {
                output[chipset.Name] = chipset;
            }
            return output;
        }


        public static Dictionary<string, ChipJSONData> MakeChipsDict(this Chipset chipset)
        {
            var output = chipset.GetAllChipsAndSubChips().Select(c => new ChipJSONData(c)).ToDict();
            return output;
        }



        public static List<ChipJSONData> FetchJSON(this Dictionary<string, ChipJSONData> chipJObjects,List<IChip> chips)
        {
            return chips.Select(chip => chipJObjects[chip.Name]).ToList();
        }

        public static IEnumerable<ChipJSONData> ChipsWithOutput(this Dictionary<string, ChipJSONData> chipsDict)
        {
            return chipsDict.Values.Where(c => c.BlockData.HasOutput);
        }

        public static IEnumerable<(ChipJSONInputData,ChipJSONData)> GetPinsWithUndefinedInput(this Dictionary<string, ChipJSONData> chipsDict)
        {
            return chipsDict.Values.SelectMany(c => c.Inputs.Where(i => i.InputType == InputOptionType.Undefined).Select(i => (i, c)));
        }

        public static CubeTemplate JSONRepToTemplate(string inputValue)
        {
            var splits = inputValue.Split('|');
            if (splits.Length != 2)
            {
                throw new Exception("CubeTemplate chip input parsing error: " + inputValue);
            }

            var name = splits[0];

            if(splits[1].Equals("Main"))
            {
                return new CubeTemplateMainPlaceholder(name);
            }

            var version = int.Parse(splits[1]);
            var templateObj = Templates.Database[name][version];
            return templateObj;
        }

        public static string TemplateToJSONRep(CubeTemplate template)
        {
            return template.Versions.Name + '|' + template.Version;
        }

        public static List<string> GetSelectedInputTypes(this BlockTop block)
        {
            var output = block.InputSections.Select(s => s.CurrentlySelected.BaseType).ToList();
            return output;
        }
    }
}
