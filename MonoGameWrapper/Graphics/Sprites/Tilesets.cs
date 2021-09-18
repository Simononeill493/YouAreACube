using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Tilesets
    {
        public const string TileEyesDirectory = "Sprites/Tilesets/Eyes";
        public const string TileBodiesDirectory = "Sprites/Tilesets/Bodies";
        public const string TileMainDirectory = "Sprites/Tilesets/Main";
        public const string TileAttach4Directory = "Sprites/Tilesets/Attach4";

        //public static Dictionary<CubeSpriteDataType, List<string>> SpritesByType;
        public static Dictionary<string, Attach4SpriteSet> Attach4Spritesets;

        public static List<string> GeneralSprites;
        public static List<string> EyeSprites;
        public static List<string> BodySprites;
        public static List<string> Attach4Sprites;

        public static List<(string, CubeSpriteDataType)> CanBeFullBodySprites;

        public static List<(string,string)> LoadTilesets(string directory)
        {
            var tileSprites = new List<(string fullName, string friendlyName)>();
            var tileSpritesDict = LoadSpritesFromFile();

            Attach4Sprites = tileSpritesDict[TileAttach4Directory];
            EyeSprites = tileSpritesDict[TileEyesDirectory];
            BodySprites = tileSpritesDict[TileBodiesDirectory];
            GeneralSprites = tileSpritesDict[TileMainDirectory];

            Attach4Spritesets = MakeAttach4Spritesets(Attach4Sprites);
            var allAttach4NamesAndPaths = Attach4Spritesets.Values.SelectMany(a => a.GetSpriteNamesAndPaths());

            tileSprites.AddRange(GetBothNames(EyeSprites, TileEyesDirectory));
            tileSprites.AddRange(GetBothNames(BodySprites, TileBodiesDirectory));
            tileSprites.AddRange(GetBothNames(GeneralSprites, TileMainDirectory));
            tileSprites.AddRange(allAttach4NamesAndPaths);

            CanBeFullBodySprites = new List<(string, CubeSpriteDataType)>();
            CanBeFullBodySprites.AddRange(GeneralSprites.Select(s => (s, CubeSpriteDataType.SingleSprite)));
            CanBeFullBodySprites.AddRange(BodySprites.Select(s => (s, CubeSpriteDataType.SingleSprite)));
            CanBeFullBodySprites.AddRange(Attach4Sprites.Select(s => (s, CubeSpriteDataType.Attach4)));

            return tileSprites;
        }

        public static Dictionary<string, Attach4SpriteSet> MakeAttach4Spritesets(List<string> attach4Sprites)
        {
            var attach4Spritesets = new Dictionary<string, Attach4SpriteSet>();

            foreach (var spriteName in attach4Sprites)
            {
                attach4Spritesets[spriteName] = new Attach4SpriteSet(spriteName);
            }

            return attach4Spritesets;
        }


        public static List<(string,string)> GetBothNames(List<string> friendlyNames,string path)
        {
            return friendlyNames.Select(n => (path + '/' + n, n)).ToList();
        }


        public static Dictionary<string, List<string>> LoadSpritesFromFile()
        {
            var tileSprites = File.ReadAllLines(ConfigFiles.TileSpritesPath).Where(l => l.Length > 0);
            var tileSpritesDict = new Dictionary<string, List<string>>();
            var spritePath = "";

            foreach (var spriteName in tileSprites)
            {
                if (spriteName[0].Equals('/'))
                {
                    spritePath = Sprites.TileSpritesRootDirectory + spriteName;
                    tileSpritesDict[spritePath] = new List<string>();
                    continue;
                }

                tileSpritesDict[spritePath].Add(spriteName);
            }

            return tileSpritesDict;
        }
    }
}
