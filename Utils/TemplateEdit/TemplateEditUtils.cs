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
            else if (bottom.Equals("AnyCube") & (top.Equals("Cube") | top.Equals("SurfaceCube") | top.Equals("GroundCube") | top.Equals("EphemeralCube")))
            {
                return true;
            }
            else if (bottom.Equals("List<Variable>") & top.StartsWith("List<"))
            {
                return true;
            }

            return false;
        }


        public static IEnumerable<BlockTopWithOutput> GetChipsWithOutput(List<BlockTop> chips) => chips.Where(c => c.HasOutput).Cast<BlockTopWithOutput>();




        public static Blockset PrepareBlocksetForEditPane(CubeTemplate template,BlockEditPane pane)
        {
            var json = Parser_ChipsetToJSON.ParseChipsetToJson(template.Chipset);
            var newBlockset = Parser_JSONToBlockset.ParseJsonToBlockset(json, pane);

            newBlockset.RefreshAll();
            newBlockset.SetLocationConfig(pane.ActualLocation + new IntPoint(10, 10), CoordinateMode.Absolute, centered: false);
            newBlockset.UpdateDimensionsCascade(pane.ActualLocation, pane.GetCurrentSize());
            
            return newBlockset;
        }
    }
}
