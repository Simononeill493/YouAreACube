﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class ConfigFiles
    {
        public static string ConfigFileDefaultLocation = @"C:\Users\Simon\Desktop\Cube\Cube\Simon_Data";
        public static string ConfigFileName = @"Config.txt";

        public static string SaveDirectoryDefaultLocation = @"C:\Users\Simon\Desktop\Cube\Cube";
        public static string SaveDirectoryName = @"Saves";
        public static string SaveExtension = ".cubesave";

        public static string TemplatesFileDefaultLocation = @"C:\Users\Simon\Desktop\Cube\Cube\Simon_Data";
        public static string TemplatesFileName = @"templates.txt";

        public static string ChipsFileDefaultLocation = @"C:\Users\Simon\Desktop\Cube\Cube\Simon_Data";
        public static string ChipsFileName = @"BuiltInChips.txt";

        public static string TemplatesFilePath;
        public static string ChipsFilePath;
        public static string SaveDirectoryPath;

        public static void Init()
        {
            _loadConfigFile();

            TemplatesFilePath = FileUtils.GetFilePath(TemplatesFileDefaultLocation, TemplatesFileName);
            ChipsFilePath = FileUtils.GetFilePath(ChipsFileDefaultLocation, ChipsFileName);
            SaveDirectoryPath = _getOrMakeSaveDirectory();

            if (TemplatesFilePath == null)
            {
                throw new FileNotFoundException("Templates.txt not found. Please add Templates.txt to the application directory.");
            }
            if (ChipsFilePath == null)
            {
                throw new FileNotFoundException("BuiltInChips.txt not found. Please add BuiltInChips.txt to the application directory.");
            }
        }

        private static string _getOrMakeSaveDirectory()
        {
            var existingPath = FileUtils.GetFilePath(SaveDirectoryDefaultLocation, SaveDirectoryName);
            if(existingPath!=null)
            {
                return existingPath;

            }

            var newDirPath = Path.Combine(Directory.GetCurrentDirectory(), SaveDirectoryName);
            Directory.CreateDirectory(newDirPath);
            return newDirPath;
        }

        private static void _loadConfigFile()
        {
            var configPath = FileUtils.GetFilePath(ConfigFileDefaultLocation, ConfigFileName);
            if (configPath == null)
            {
                Console.WriteLine("Config.txt not found. Using default configuration.");
                return;
            }

            var configItems = _parseConfigFile(configPath);
            _setConfigDataForStaticClass(typeof(Config), configItems);
            _setConfigDataForStaticClass(typeof(ConfigFiles), configItems);
        }

        private static void _setConfigDataForStaticClass(Type t, List<(string Key, string Value)> configItems)
        {
            var staticTypeFields = t.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            foreach (var staticField in staticTypeFields)
            {
                var configItem = _getMatchingConfigEntry(configItems, staticField.Name);
                if (configItem.Key == null)
                {
                    continue;
                }

                var value = TypeUtils.ParseType(staticField.FieldType, configItem.Value);
                if (value == null)
                {
                    throw new Exception("Tried to load the field '" + configItem.Key + "' from Config.txt, but failed to parse its value, '" + configItem.Value + "' into the required type '" + staticField.FieldType.FullName + "'");
                }

                staticField.SetValue(null, value);
            }
        }

        private static List<(string Key, string Value)> _parseConfigFile(string configPath)
        {
            var output = new List<(string, string)>();

            var lines = File.ReadAllLines(configPath);
            foreach (var line in lines)
            {
                if (line.Trim().Length == 0)
                {
                    continue;
                }

                var splits = line.Split('=');
                if (splits.Length != 2)
                {
                    throw new Exception("Tried to parse the line '" + line + "' from Config.txt, but string was not in the correct format: '[key]=[value]'");
                }

                output.Add((splits[0].Trim(), splits[1].Trim()));
            }

            return output;
        }

        private static (string Key, string Value) _getMatchingConfigEntry(List<(string Key, string Value)> configItems, string fieldName) => configItems.FirstOrDefault(c => c.Key.ToLower().Equals(fieldName.ToLower()));

    }
}
