using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class Blockset
    {
        public void AssertSanityTest()
        {
            if (SanityTest().Count > 0)
            {
                throw new Exception("Blockset failed sanity test");
            }
        }

        public List<string> SanityTest()
        {
            var output = new List<string>();
            
            if(Name==null)
            {
                output.Add("Name is null");
            }

            foreach(var block in Blocks)
            {
                var result = block.SanityTest();
                output.AddRange(result);
            }

            return output;
        }
    }
}
