using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class ChipDatabase
    {
        public static Dictionary<string,ChipData> BuiltInChips;

        public static void Load() => BuiltInChips = _loadBuiltInChips();
        public static IEnumerable<ChipData> SearchChips(string searchTerm) => BuiltInChips.Values.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()));

        private static Dictionary<string, ChipData> _loadBuiltInChips()
        {
            var data = FileUtils.LoadJson(ConfigFiles.BuiltInChipsFilePath)["chips"];
            return BuiltInChipParser.ParseChips(data);
        }
    }
}
