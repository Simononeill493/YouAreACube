using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class BlockUtils
    {
        public static List<string> NoTypeArguments => new List<string>() { "" };

        public static BlockTop GenerateBlockFromBlockData(BlockData data, string name = "")
        {
            var initialDrawLayer = ManualDrawLayer.Zero;

            if (data.Name.Equals("If"))
            {
                return new BlockTopSwitch(name, initialDrawLayer, data, new List<string>() { "Yes", "No" });
            }
            if (data.Name.Equals("KeySwitch"))
            {
                return new BlockTopSwitch(name, initialDrawLayer, data, new List<string>() { });
            }
            else if (data.HasOutput)
            {
                return new BlockTopWithOutput(name, initialDrawLayer, data);
            }
            else
            {
                return new BlockTop(name, initialDrawLayer, data);
            }
        }
    }
}
