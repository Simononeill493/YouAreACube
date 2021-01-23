using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class SaveManager
    {
        public static Save FreshSave()
        {
            var seed = new Random(1);

            var kernel = new Kernel();
            var world = WorldGen.GenerateFreshWorld(seed);

            var save = new Save(kernel, world);
            return save;
        }

        public static void SaveToFile(Save save,string name)
        {
            FileUtils.SaveBinary(save, Config.SaveDirectory, name, Config.SaveExtension);
        }
        public static Save LoadFromFile(string name)
        {
            var save = FileUtils.LoadBinary<Save>(Config.SaveDirectory, name);
            return save;
        }
    }
}
