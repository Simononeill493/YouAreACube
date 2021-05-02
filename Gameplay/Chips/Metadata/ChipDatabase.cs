using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class ChipDatabase
    {
        public static Dictionary<string,GraphicalChipData> GraphicalChips;

        public static void Load() => GraphicalChips = _loadGraphicalChips();
        public static IEnumerable<GraphicalChipData> SearchChips(string searchTerm) => GraphicalChips.Values.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()));

        private static Dictionary<string, GraphicalChipData> _loadGraphicalChips()
        {
            var data = FileUtils.LoadJson(ConfigFiles.GraphicalChipsPath)["chips"];
            return GraphicalChipParser.ParseChips(data);
        }
    }
}
