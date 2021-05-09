using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateEditUtils
    {
        public static bool IsValidInputFor(string top, string bottom)
        {
            if (bottom.Equals(top) | bottom.Equals("Variable"))
            {
                return true;
            }
            else if (bottom.Equals("AnyBlock") & (top.Equals("Block") | top.Equals("SurfaceBlock") | top.Equals("GroundBlock") | top.Equals("EphemeralBlock")))
            {
                return true;
            }
            else if (bottom.Equals("List<Variable>") | top.StartsWith("List<"))
            {
                return true;
            }

            return false;
        }


        public static IEnumerable<ChipTopWithOutput> GetChipsWithOutput(List<ChipTop> chips) => chips.Where(c => c.HasOutput).Cast<ChipTopWithOutput>();




        public static EditableChipset PrepareChipsetForEditPane(BlockTemplate template,ChipEditPane pane)
        {
            var json = ChipBlockToJSONParser.ParseBlockToJson(template.ChipBlock);
            var newChipset = JSONToEditableChipsetParser.ParseJsonToEditableChipset(json, pane);

            newChipset.RefreshAll();
            newChipset.SetLocationConfig(pane.ActualLocation + new IntPoint(10, 10), CoordinateMode.Absolute, centered: false);
            newChipset.UpdateDimensionsCascade(pane.ActualLocation, pane.GetCurrentSize());
            
            return newChipset;
        }
    }
}
