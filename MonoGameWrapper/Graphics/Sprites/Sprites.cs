using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Sprites
    {
        public static List<(string fullName, string friendlyName)> AllSprites;

        public static string TileSpritesRootDirectory = "Sprites/Tilesets";
        public static string MenuSpritesDirectory = "Sprites/Static/Menu";
        public static string TitleSpritesDirectory = "Sprites/Static/Title";

        public static void Init()
        {
            AllSprites = new List<(string,string)>();
            AllSprites.AddRange(Tilesets.LoadTilesets(TileSpritesRootDirectory));

            AllSprites.AddRange(LoadStaticSprites(MenuSpritesDirectory,typeof(MenuSprites)));
            AllSprites.AddRange(LoadStaticSprites(TitleSpritesDirectory, typeof(TitleAnimationSprites)));
        }

        public static List<(string, string)> LoadStaticSprites(string directory,Type spriteHolder)
        {
            var sprites = new List<(string fullname, string friendlyName)>();
            var spriteFieldProperties = spriteHolder.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            foreach (var property in spriteFieldProperties.ToList())
            {
                var spriteNameFriendly = (string)property.GetValue(null);
                var spriteNameFull = directory + '/' + spriteNameFriendly;

                sprites.Add((spriteNameFull, spriteNameFriendly));
            }

            return sprites;
        }

    }
}
