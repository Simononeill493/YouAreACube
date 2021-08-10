using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BuiltInSprites
    {
        public static string MenuSpritesDirectory = "Sprites/Menus/";
        public static string TileSpritesDirectory = "Sprites/Tiles/";

        public static List<(string fullName,string friendlyName)> AllSprites;

        public static void Init()
        {
            AllSprites = new List<(string,string)>();
            BuiltInMenuSprites.ConfigureMenuSprites(AllSprites);

            _configureTileSprites(AllSprites);
        }


        private static void _configureTileSprites(List<(string fullname, string friendlyName)> allSprites)
        {
            var tileSprites = File.ReadAllLines(ConfigFiles.TileSpritesPath);
            foreach(var spriteName in tileSprites)
            {
                allSprites.Add((TileSpritesDirectory + spriteName, spriteName));
            }
        }
    }
}
