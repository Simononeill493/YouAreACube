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
            var player = Templates.GenerateSurface("BasicPlayer",0);
            var world = WorldGen.GenerateEmptyWorld(0);

            WorldGen.AddPlayer(world, player);
            WorldGen.AddEntities(world);

            var kernel = new Kernel();
            kernel.SetHost(player);

            return new Save(kernel, world);
        }

        public static void SaveToFile(Save save) => FileUtils.SaveBinary(save, ConfigFiles.SaveDirectoryPath, save.Name, ConfigFiles.SaveExtension);
        public static Save LoadFromFile(string name) => FileUtils.LoadBinary<Save>(ConfigFiles.SaveDirectoryPath, name);
    }
}
