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

            try
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
            TypeUtils.Load();
            ChipDatabase.Load();
            RandomUtils.Init(1);
            DirectionUtils.Init();
            Templates.Load();

            TestCode();

            var game = new MonoGameWindow();
            game.Run();
        }

        public static void TestCode()
        {
            //var world = WorldGen.GenerateEmptyWorld(59);
        }
    }
}
