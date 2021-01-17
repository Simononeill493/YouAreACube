using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Templates
    {
        public static GroundBlock VoidBlock;

        public static void Load() 
        {
            VoidBlock = new GroundBlock() { Background = "Black" };
        }
    }
}
