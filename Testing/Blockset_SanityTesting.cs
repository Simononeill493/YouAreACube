using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    /*partial class Blockset
    {
        public void AssertSanityTest()
        {
            var result = SanityTest();
            if (result.Count > 0)
            {
                throw new Exception("Blockset failed sanity test");
            }
        }

        public List<string> SanityTest((List<string> blocksetNames, List<string> blockNames) names = default)
        {
            if(names == default)
            {
                var subBlocksets = GetThisAndSubBlocksets();

                names.blocksetNames = subBlocksets.Select(b => b.Name).ToList();
                names.blockNames = subBlocksets.SelectMany(b => b.Blocks.Select(bl=>bl.Name)).ToList();
            }

            var output = new List<string>();
            var name = "unnamed_blockset";

            if (Name==null)
            {
                output.Add("Name is null");
            }
            else
            {
                name = Name;
            }

            foreach(var block in Blocks)
            {
                var result = block.SanityTest(names);
                output.AddRange(result.Select(r => name + ": " + r));
            }

            return output;
        }
    }*/
}
