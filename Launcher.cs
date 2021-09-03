 using System;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace IAmACube
{
    public static class Launcher
    {
        [STAThread]
        public static void Main(string[] args)
        {
#if DEBUG
            Run();
            return;
#endif

#pragma warning disable CS0162 // Unreachable code not an issue here
            try
#pragma warning restore CS0162
            {
                Run();
            }
            catch(Exception e)
            {
                var path = Directory.GetCurrentDirectory();
                var exceptionLogPath = Path.Combine(path, "Cube_exception_log.txt");

                File.AppendAllText(exceptionLogPath, "\n\n" + e.GetType().ToString() + ":\t" + e.Message);
                File.AppendAllText(exceptionLogPath, "\n" + e.StackTrace);
                Application.Exit();
                return;
            }

        }

        private static void Run()
        {
            Config.Init();
            ColorLookup.Init();
            BuiltInSprites.Init();
            RandomUtils.Init();
            DirectionUtils.Init();
            InGameTypeUtils.Init();

            TypeUtils.Load();
            BlockDataDatabase.Load();
            BlockDataGenerationTester.Test();
            Templates.Load();
            CrystalDatabase.Load();

            TestCode();

            var game = new MonoGameWindow();
            game.Run();
        }

        public static void TestCode()
        {

        }
    }
}
