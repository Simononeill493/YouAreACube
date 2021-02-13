﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Config
    {
        public const int TickCycleLength = 60;
        public const int TileSizeActual = 16;

        public const int SectorSize = 16;
        public const bool EnableFrameCounter = false;

        public const string SaveDirectory = @"C:\Users\Simon\Desktop\Cube\Cube\Saves";
        public const string SaveExtension = ".cubesave";

        public const string TemplatesFile = @"C:\Users\Simon\Desktop\Cube\Cube\Simon_Sprites\templates.txt";

        public static bool KernelUnlimitedEnergy = true;

        public static int MenuItemScale = 3;
        public static int MenuItemTextScale = 2;

    }
}
