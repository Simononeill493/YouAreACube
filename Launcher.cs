 using System;
using System.Windows.Forms;
using System.Linq;

namespace IAmACube
{
    public static class Launcher
    {
        [STAThread]
        public static void Main(string[] args)
        {
            RandomUtils.Init(1);
            DirectionUtils.Init();
            Templates.Load();

            TestCode();

            var game = new MonoGameWindow();
            game.Run();
        }

        public static void TestCode()
        {
            var world = WorldGen.GenerateEmptyWorld(59);
        }
    }
}
