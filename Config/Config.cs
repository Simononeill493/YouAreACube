using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Config
    {
        public const int TickCycleLength = 60;
        public const int TileSizePixels = 16;

        public const int SectorSize = 16;
        public const bool EnableFrameCounter = false;

        public const string SaveDirectory = @"C:\Users\Simon\Desktop\Cube\Cube\Saves";
        public const string SaveExtension = ".cubesave";

        public const string TemplatesFile = @"C:\Users\Simon\Desktop\Cube\Cube\Simon_Data\templates.txt";
        public const string ChipsFile = @"C:\Users\Simon\Desktop\Cube\Cube\Simon_Data\BuiltInChips.txt";

        public static bool KernelUnlimitedEnergy = true;

        public static int MenuItemScale = 4;

        public static Color DefaultTextColor = Color.Black;

        public const int ScreenDefaultWidth = 1024;//1024
        public const int ScreenDefaultHeight = 768;//768

    }
}
