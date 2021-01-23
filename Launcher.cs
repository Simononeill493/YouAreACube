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
            TestCode();

            RandomUtils.Init(1);
            DirectionUtils.Init();
            Templates.Load();

            var game = new MonoGameWindow();
            game.Run();
        }

        public static void TestCode()
        {
            ChipTester.Go();
        }
    }
}
