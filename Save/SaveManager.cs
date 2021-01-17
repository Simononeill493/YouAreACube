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
            var savePath = Path.Combine(Config.SaveDirectory, name) + Config.SaveExtension;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(savePath, FileMode.Create, FileAccess.Write);

            formatter.Serialize(stream, save);
            stream.Close();
        }

        public static Save LoadFromFile(string name)
        {
            var savePath = Path.Combine(Config.SaveDirectory, name);
            IFormatter formatter = new BinaryFormatter();

            var stream = new FileStream(savePath, FileMode.Open, FileAccess.Read);
            Save save = (Save)formatter.Deserialize(stream);
            stream.Close();

            return save;
        }
    }
}
