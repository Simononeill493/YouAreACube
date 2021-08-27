using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class BlockDataUtils
    {
        public static List<string> NoTypeArguments => new List<string>() { "" };

        public static BlockTop GenerateBlockFromBlockData(BlockData data, string name = "")
        {
            var initialDrawLayer = ManualDrawLayer.Zero;

            if (IsIfBlock(data))
            {
                return new BlockTopSwitch(name, initialDrawLayer, data, new List<string>() { "Yes", "No" });
            }
            else if (data.Name.Equals("KeySwitch"))
            {
                return new BlockTopSwitch(name, initialDrawLayer, data, new List<string>() { });
            }
            else if (data.HasOutput)
            {
                return new BlockTopWithOutput(name, initialDrawLayer, data);
            }
            else if(data.Name == "SetVariable")
            {
                return new BlockTopVariableSetter(name, initialDrawLayer, data);
            }
            else
            {
                return new BlockTop(name, initialDrawLayer, data);
            }
        }

        public static bool IsIfBlock(BlockData data)
        {
            return data.Name.Equals("If") | data.Name.Equals("IfPercentage");
        }
    }
}
