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
        public static string TileMainDirectory = "Sprites/Tiles";
        public static string TileAttach4Directory = "Sprites/Attach4";

        public static List<string> EyeSprites;
        public static List<string> BodySprites;
        public static Dictionary<string, Attach4SpriteSet> Attach4Sprites;

        public static void ConfigureTileSprites(List<(string fullName, string friendlyName)> allSprites, string directory)
        {
            var tileSprites = File.ReadAllLines(ConfigFiles.TileSpritesPath).Where(l=>l.Length>0);
            var tileSpritesDict = new Dictionary<string, List<string>>();
            var spritePath = "";

            foreach (var spriteName in tileSprites)
            {
                if(spriteName[0].Equals('/'))
                {
                    spritePath = "Sprites" + spriteName;
                    tileSpritesDict[spritePath] = new List<string>();

                    continue;
                }

                tileSpritesDict[spritePath].Add(spriteName);
            }


            EyeSprites = tileSpritesDict[TileEyesDirectory];
            BodySprites = tileSpritesDict[TileBodiesDirectory];

            allSprites.AddRange(GetBothNames(EyeSprites,TileEyesDirectory));
            allSprites.AddRange(GetBothNames(BodySprites,TileBodiesDirectory));
            allSprites.AddRange(GetBothNames(tileSpritesDict[TileMainDirectory], TileMainDirectory));

            LoadAttach4Sprites(tileSpritesDict[TileAttach4Directory], allSprites);
        }

        public static void LoadAttach4Sprites(List<string> attach4Sprites, List<(string,string)> allSprites)
        {
            Attach4Sprites = new Dictionary<string, Attach4SpriteSet>();

            foreach(var spriteName in attach4Sprites)
            {
                LoadAttach4SpriteSet(TileAttach4Directory + '/' + spriteName, spriteName,allSprites);
            }
        }

        public static void LoadAttach4SpriteSet(string path,string name, List<(string fullName, string friendlyName)> allSprites)
        {
            var spriteSet = new Attach4SpriteSet(name);
            allSprites.Add((path + '/' + spriteSet.sprite1, name));

            allSprites.Add((path + '/' + spriteSet.sprite0, spriteSet.sprite0));
            allSprites.Add((path + '/' + spriteSet.sprite1, spriteSet.sprite1));
            allSprites.Add((path + '/' + spriteSet.sprite2_1, spriteSet.sprite2_1));
            allSprites.Add((path + '/' + spriteSet.sprite2_2, spriteSet.sprite2_2));
            allSprites.Add((path + '/' + spriteSet.sprite3, spriteSet.sprite3));
            allSprites.Add((path + '/' + spriteSet.sprite4, spriteSet.sprite4));

            Attach4Sprites[name] = spriteSet;
        }



        public static List<(string,string)> GetBothNames(List<string> friendlyNames,string path)
        {
            return friendlyNames.Select(n => (path + '/' + n, n)).ToList();
        }
    }
}
