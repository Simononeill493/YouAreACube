﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class GraphicalChipDatabase
    {
        public static Dictionary<string,GraphicalChipData> GraphicalChips;
        public static IEnumerable<GraphicalChipData> SearchableChips;

        public static void Load()
        {
            GraphicalChips = _loadGraphicalChips();
            SearchableChips = GraphicalChips.Values.Where(c => c.BaseMappingChip == null);
        }

        public static IEnumerable<GraphicalChipData> SearchChips(string searchTerm) => SearchableChips.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()));

        private static Dictionary<string, GraphicalChipData> _loadGraphicalChips()
        {
            var data = FileUtils.LoadJson(ConfigFiles.GraphicalChipsPath)["chips"];
            var chips =  GraphicalChipParser.ParseChips(data);

            foreach (var chip in chips.Values.ToList())
            {
                foreach(var subMapping in chip.GetAllSubMappings())
                {
                    chips[subMapping.Name] = subMapping;
                    subMapping.Init();
                    subMapping.BaseMappingChip = chip;
                }
            }

            return chips;
        }
    }
}