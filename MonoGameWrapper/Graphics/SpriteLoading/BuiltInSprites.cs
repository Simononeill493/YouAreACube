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
        public static string MenuSpritesDirectory = "Sprites/Menus";
        public static string TileSpritesDirectory = "Sprites";


        public static List<(string fullName,string friendlyName)> AllSprites;

        public static void Init()
        {
            AllSprites = new List<(string,string)>();
            BuiltInMenuSprites.ConfigureMenuSprites(AllSprites,MenuSpritesDirectory);
            BuiltInTileSprites.ConfigureTileSprites(AllSprites, TileSpritesDirectory);
        }


    }
}
