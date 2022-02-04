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
        public const int NumChipInputPins = 3;

        public static int ScreenDefaultWidth = 1280;
        public static int ScreenDefaultHeight = 960;

        public static int DefaultSectorSize = 64;//25
        public static int TileSizePixels = 16;
        public static int TickCycleLength = 60;

        public static bool EnableFrameCounter = false;

        public static bool KernelUnlimitedEnergy = false;
        public static bool KernelHostInvincible = false;
        public static bool KernelLearnAllTemplates = false;
        public static bool KernelLearnAllBlocks = false;

        public static void SetGodMode()
        {
            KernelUnlimitedEnergy = true;
            KernelHostInvincible = true;
            KernelLearnAllTemplates = true;
            KernelLearnAllBlocks = true;
        }

        public static int DefaultMenuItemScale = 4;
        public static Color DefaultTextColor = Color.Black;

        public static int DefaultBlockSpeed = 30;


        public static void Init() => ConfigFiles.Init();
    }
}
