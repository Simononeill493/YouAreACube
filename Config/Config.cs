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
        public const int TickCycleLength = 60;
        public const int TileSizePixels = 16;

        public const int SectorSize = 16;
        public const bool EnableFrameCounter = false;

        public const string SaveDirectoryDefaultLocation = @"C:\Users\Simon\Desktop\YouAreACube";
        public const string SaveDirectoryName = @"Saves";
        public const string SaveExtension = ".cubesave";

        public const string TemplatesFileDefaultLocation = @"C:\Users\Simon\Desktop\YouAreACube\Simon_Data";
        public const string TemplatesFileName= @"templates.txt";

        public const string ChipsFileDefaultLocation = @"C:\Users\Simon\Desktop\YouAreACube\Simon_Data";
        public const string ChipsFileName = @"BuiltInChips.txt";

        public static bool KernelUnlimitedEnergy = true;

        public static int MenuItemScale = 4;

        public static Color DefaultTextColor = Color.Black;

        public const int ScreenDefaultWidth = 1024;//1024
        public const int ScreenDefaultHeight = 768;//768

        public static void Init()
        {
            TemplatesFilePath = _getFilePath(TemplatesFileDefaultLocation, TemplatesFileName);
            ChipsFilePath = _getFilePath(ChipsFileDefaultLocation, ChipsFileName);
            SaveDirectoryPath = _getSaveDirectoryPath();

            if(TemplatesFilePath==null)
            {
                throw new FileNotFoundException("Templates.txt not found. Please add Templates.txt to the application directory.");
            }
            if (ChipsFilePath == null)
            {
                throw new FileNotFoundException("BuiltInChips.txt not found. Please add BuiltInChips.txt to the application directory.");
            }
        }

        private static string _getFilePath(string defaultLocation,string fileName)
        {
            var defaultPath = Path.Combine(defaultLocation, fileName);
            if(File.Exists(defaultPath) | Directory.Exists(defaultPath))
            {
                return defaultPath;
            }

            var workingDirPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            if (File.Exists(workingDirPath) | Directory.Exists(workingDirPath))
            {
                return workingDirPath;
            }

            return null;
        }

        private static string _getWorkingDirFilePath(string fileName)
        {
            var workingDir = Directory.GetCurrentDirectory();
            var workingDirContents = FileUtils.GetDirectoryContents(workingDir);

            if (workingDirContents.Contains(fileName))
            {
                var path = Path.Combine(workingDir, fileName);
                return path;
            }

            return null;
        }

        private static string _getSaveDirectoryPath()
        {
            var existingPath = _getFilePath(SaveDirectoryDefaultLocation, SaveDirectoryName);
            if(existingPath==null)
            {
                var workingDir = Directory.GetCurrentDirectory();
                var newDirPath = Path.Combine(workingDir, SaveDirectoryName);
                Directory.CreateDirectory(newDirPath);
                return newDirPath;
            }

            return existingPath;
        }

        public static string TemplatesFilePath;
        public static string ChipsFilePath;
        public static string SaveDirectoryPath;
    }
}
