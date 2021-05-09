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
        public static (Kernel Kernel, World World) GenerateTestSave(string name = "")
        {
            var kernel = GenerateTestKernel(name);
            var world = GenerateTestWorld(name);

            AddKernelToWorld(kernel, world);

            return (kernel, world);
        }

        public static Kernel GenerateTestKernel(string name = "")
        {
            var kernel = new Kernel();
            kernel.Name = name;

            return kernel;
        }

        public static World GenerateTestWorld(string name = "")
        {
            var world = WorldGen.GenerateEmptyWorld(0);
            world.Name = name;

            world.GetAllSectors().ForEach(s=> WorldGen.AddEntities(s,world.Random));

            return world;
        }

        public static void AddKernelToWorld(Kernel kernel, World world)
        {
            var player = Templates.GenerateSurface("BasicPlayer", 0);

            WorldGen.AddPlayer(world, player);
            kernel.SetHost(player);
        }


        public static void SaveKernel(Kernel kernel) => FileUtils.SaveBinary(kernel, ConfigFiles.SaveDirectory, kernel.Name, ConfigFiles.SaveKernelExtension);
        public static void SaveWorld(World world) => FileUtils.SaveBinary(world, ConfigFiles.SaveDirectory, world.Name, ConfigFiles.SaveWorldExtension);


        public static Kernel LoadKernel(string name) => FileUtils.LoadBinary<Kernel>(ConfigFiles.SaveDirectory, name, ConfigFiles.SaveKernelExtension);
        public static World LoadWorld(string name) => FileUtils.LoadBinary<World>(ConfigFiles.SaveDirectory, name, ConfigFiles.SaveWorldExtension);

    }
}
