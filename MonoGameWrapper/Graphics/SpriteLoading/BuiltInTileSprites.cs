using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BuiltInTileSprites
    {
        public static string TileEyesDirectory = "Sprites/Eyes";
        public static string TileBodiesDirectory = "Sprites/Bodies";

        public static List<string> EyeSprites;
        public static List<string> BodySprites;

        public static void ConfigureTileSprites(List<(string fullname, string friendlyName)> allSprites, string directory)
        {
            var currentDirectory = directory;
            var tileSprites = File.ReadAllLines(ConfigFiles.TileSpritesPath).Where(l=>l.Length>0);
            foreach (var spriteName in tileSprites)
            {
                if(spriteName[0].Equals('/'))
                {
                    currentDirectory = directory + spriteName + '/';
                    continue;
                }
                allSprites.Add((currentDirectory + spriteName, spriteName));
            }


            EyeSprites = allSprites.Where(kvp => kvp.fullname.Contains(TileEyesDirectory)).Select(kvp => kvp.friendlyName).ToList();
            BodySprites = allSprites.Where(kvp => kvp.fullname.Contains(TileBodiesDirectory)).Select(kvp => kvp.friendlyName).ToList();
        }
    }
}
