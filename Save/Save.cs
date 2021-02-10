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
        public string Name;
        public Kernel Kernel;
        public World World;

        public Save(Kernel kernel,World world,string name = "_null_")
        {
            Name = name;
            Kernel = kernel;
            World = world;
        }
    }
}
