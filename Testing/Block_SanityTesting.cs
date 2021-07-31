using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public partial class BlockTop
    {
        public List<string> SanityTest()
        {
            var output = new List<string>();

            if (Name == null)
            {
                output.Add("Name is null");
            }

            foreach (var inputSection in _inputSections)
            {
                var result = inputSection.SanityTest();
                output.AddRange(result);
            }


            foreach (var subBlockset in GetSubBlocksets())
            {
                var result = subBlockset.SanityTest();
                output.AddRange(result);
            }

            return output;
        }

    }
}
