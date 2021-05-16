using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Config
    {
        public static int ScreenDefaultWidth = 1024;
        public static int ScreenDefaultHeight = 768;

        public static int DefaultSectorSize = 64;//25
        public static int TileSizePixels = 16;
        public static int TickCycleLength = 60;

        public static bool EnableFrameCounter = false;
        public static bool KernelUnlimitedEnergy = true;
        public static bool KernelLearnAllTemplates = true;

        public static int MenuItemScale = 4;
        public static Color DefaultTextColor = Color.Black;

        public static int DefaultBlockSpeed = 30;


        public static void Init() => ConfigFiles.Init();
    }
}
