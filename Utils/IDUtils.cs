using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class IDUtils
    {
        public static int GenerateCubeID()
        {
             return RandomUtils.R.Next(0, 99999);
        }

        public static string GenerateBlockID(BlockData data)
        {
            return data.Name + "_" + RandomUtils.R.Next(0, 99999);
        }

        public static string GenerateBlocksetID()
        {
            return "Blockset_" + RandomUtils.R.Next(0, 99999);
        }
    }
}
