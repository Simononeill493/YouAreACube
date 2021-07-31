using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public partial class BlockInputSection
    {
        public List<string> SanityTest()
        {
            var output = new List<string>();

            if(CurrentlySelected==null)
            {
                output.Add("No input option selected");
            }

            return output;
        }

    }
}
