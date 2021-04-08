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
            var player = Templates.GenerateSurface("BasicPlayer");

            var world = WorldGen.GenerateEmptyWorld(0);
            WorldGen.AddPlayer(world, player);
            WorldGen.AddEntities(world);

            var kernel = new Kernel() { Host = player };
            var save = new Save(kernel, world);

            return save;
        }

        public static void SaveToFile(Save save)
        {
            FileUtils.SaveBinary(save, Config.SaveDirectoryPath, save.Name, Config.SaveExtension);
        }
        public static Save LoadFromFile(string name)
        {
            var save = FileUtils.LoadBinary<Save>(Config.SaveDirectoryPath, name);
            return save;
        }
    }
}
