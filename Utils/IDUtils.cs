using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class IDUtils
    {
        public static int GenerateBlockID()
        {
             return RandomUtils.R.Next(0, 99999);
        }
    }
}
