using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class Save
    {
        public Kernel Kernel;
        public World World;

        public Save(Kernel kernel,World world)
        {
            Kernel = kernel;
            World = world;
        }
    }
}
