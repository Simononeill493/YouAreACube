using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class BlockDataUtils
    {
        public static bool IsIfBlock(this BlockData data) => data.Name.Equals("If") | data.Name.Equals("IfPercentage");
        public static bool IsSwitchBlock(this BlockData data) => IsIfBlock(data) | data.Name.Equals("KeySwitch");

        public static List<string> GetDefaultSwitchSections(this BlockData data)
        {
            if(IsIfBlock(data))
            {
                return new List<string>() { "Yes", "No" };
            }

            return new List<string>();
        }
        
    }
}
