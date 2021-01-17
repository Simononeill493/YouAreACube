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
            //var s = WorldGen.GenerateFreshWorld();
            Templates.Load();

            var game = new MonoGameWindow();
            game.Run();
        }
    }
}
