using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    /*public partial class BlockTop
    {
        public List<string> SanityTest((List<string> blocksetNames, List<string> blockNames) names)
        {
            var output = new List<string>();
            var name = "unnamed_block";

            if (Name == null)
            {
                output.Add("Block name is null");
            }
            else
            {
                name = Name;
            }

            for(int i=0;i<InputSections.Count;i++)
            {
                var result = InputSections[i].SanityTest(names);
                output.AddRange(result.Select(r => name + ", Input Section " + i + ": " + r));
            }

            foreach (var subBlockset in GetSubBlocksets())
            {
                var result = subBlockset.SanityTest(names);
                output.AddRange(result.Select(r => name + ": " + r));
            }

            return output;
        }

    }*/
}
