using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class InputCompareUtils
    {
        public static bool IsValidInputFor(string top, List<string> bottom) => bottom.Any(b => IsValidInputFor(top, b));
        public static bool IsValidInputFor(string top, string bottom)
        {
            if(top==null)
            {
                return false;
            }
            if (bottom.Equals(top) | bottom.Equals("Variable") )
            {
                return true;
            }
            else if (bottom.Equals("AnyCube") & top.Equals(nameof(Cube)) | top.Equals(nameof(SurfaceCube)) | top.Equals(nameof(GroundCube)) | top.Equals(nameof(EphemeralCube)))
            {
                return true;
            }
            else if (bottom.Equals("List<Variable>") & top.StartsWith("List<"))
            {
                return true;
            }
            else if (bottom.Equals(nameof(CubeTemplate)) & top.Equals(nameof(CubeTemplateMainPlaceholder)))
            {
                return true;
            }


            return false;
        }

        public static List<BlockInputOption_2> GetInputsFromModel(this FullModel fullModel,BlockModel block, List<string> inputTypes)
        {
            var candidates = fullModel.Blocks.Values.Where(b => IsValidInputFor(b.OutputTypeName, inputTypes)).ToList();
            var matching = candidates.Where(c => fullModel.GetAllBlocksBelow(c).Contains(block));
            return matching.Select(b => BlockInputOption_2.CreateReference(b)).ToList();
        }

        public static List<BlockModel> GetAllBlocksBelow(this FullModel fullModel,BlockModel block)
        {
            var blocks = fullModel.Blocksets.Values.Where(b => b.Blocks.Contains(block)).FirstOrDefault().Blocks;
            var index = blocks.IndexOf(block);
            var allAfter = blocks.GetRange(index, blocks.Count() - index);
            return allAfter;
        }

    }
}
