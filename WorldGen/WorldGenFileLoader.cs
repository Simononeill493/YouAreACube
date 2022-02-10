using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class WorldGenFileLoader
    {
        public const string DemoWorld1FilePath = @"C:\Users\Simon\Desktop\Cube\Cube\Simon_Data\DemoWorldFile1.txt";

        public const string TopLevelSeperator = "-";
        public const string Level2Seperator = "#";

        public const char MetaDataSecondOrderSeperator = ',';
        public const char SectorSizeXYSeperator = ',';
        public const char MetadataKeyVsValueSeperator = '=';
        public const char WorldPicEmptySpace = ' ';

        public static (IntPoint, List<Sector>, SurfaceCube) LoadDemoSectors1(WorldKernel kernel)
        {
            var lines = File.ReadAllLines(DemoWorld1FilePath).ToList();
            var topLevelSeperated = StringUtils.Seperate(lines, TopLevelSeperator);

            var (sectorSize,metaData) = ParseWorldMetaData(topLevelSeperated[0]);

            var sectors = topLevelSeperated.GetRange(1, topLevelSeperated.Count - 1);
            var sectorsList = new List<Sector>();

            foreach (var sectorLines in sectors)
            {
                var sector = SectorFromLines(sectorSize, sectorLines, kernel, metaData);
                sectorsList.Add(sector);
            }

            var player = sectorsList.FirstOrDefault(s => s.AbsoluteLocation.Equals(IntPoint.Zero)).GetPlayer();
            return (sectorSize, sectorsList, player);
        }

        public static (IntPoint sectorSize, Dictionary<string,string> topLevelMetaData) ParseWorldMetaData(List<string> lines)
        {
            var worldDataSeperated = StringUtils.Seperate(lines, Level2Seperator);
            var sectorSize = IntPoint.Parse(worldDataSeperated[0][0]);
            var metaData = ParseMetaData(worldDataSeperated[1]);

            return (sectorSize, metaData);
        }

        private static SurfaceCube GetPlayer(this Sector sector)
        {
            var player = sector.ActiveBlocks.Where(a => a.Template.Name.Contains("Player") & a.CubeMode == CubeMode.Surface).FirstOrDefault();
            return (SurfaceCube)player;
        }

        public static Sector SectorFromLines(IntPoint size,List<string> lines,Kernel worldKernel, Dictionary<string, string> topLevelMetadata)
        {
            var sections = StringUtils.Seperate(lines,Level2Seperator);
            var location = IntPoint.Parse(sections[0][0]);
            var sectorPic = sections[1];
            var descsLines = sections[2];

            if (sectorPic.Count != size.Y)
            {
                throw new Exception("World picture is the wrong size.");
            }

            var metaData = ParseMetaData(descsLines);
            foreach(var kvp in topLevelMetadata)
            {
                if(!metaData.ContainsKey(kvp.Key))
                {
                    metaData[kvp.Key] = kvp.Value;
                }
            }

            return MakeSector(sectorPic, size,location, metaData, TemplatesFromMetaData(metaData), worldKernel);
        }

        public static Sector MakeSector(List<string> sectorPic, IntPoint size,IntPoint location, Dictionary<string,string> metaData,Dictionary<char, CubeTemplate> descs,Kernel worldKernel)
        {
            var sector = WorldGen.GetEmptyTestSector(location, size);
            var worldGenGrid = new WorldGenGrid(worldKernel, size, new Random());

            var groundSprites = new List<string>() { "Grass" };
            if(metaData.ContainsKey("groundSprite"))
            {
                groundSprites = metaData["groundSprite"].Split(MetaDataSecondOrderSeperator).ToList();
            }

            var groundMask = XnaColors.ClearColorMask;
            if (metaData.ContainsKey("groundMask"))
            {
                groundMask = (XnaColors)Enum.Parse(typeof(XnaColors),metaData["groundMask"]);
            }


            for (int i=0; i<size.Y;i++)
            {
                var line = sectorPic[i];
                if (line.Length != size.X) { throw new Exception(); }

                for(int j=0;j<size.X;j++)
                {
                    var tileSymbol = line[j];
                    var tile = worldGenGrid.TilesGrid[j, i];

                    if (tileSymbol != WorldPicEmptySpace)
                    {
                        var template = descs[tileSymbol];
                        tile.Surface = template;
                    }

                    tile.TileSprite = groundSprites.GetRandom();
                    tile.TileSpriteMask = groundMask;
                }
            }

            worldGenGrid.OverlayOnSector(sector);
            WorldGen.FillEnergyBarsForSector(sector);
            return sector;
        }

        public static Dictionary<string, string> ParseMetaData(List<string> lines)
        {
            var output = new Dictionary<string, string>();
            foreach (var str in lines)
            {
                var pair = str.Split(MetadataKeyVsValueSeperator);
                var key = pair[0];
                var value = pair[1];

                output[key] = value;
            }

            return output;
        }


        public static Dictionary<char,CubeTemplate> TemplatesFromMetaData(Dictionary<string, string> topLevelMetadata)
        {
            var output = new Dictionary<char, CubeTemplate>();
            foreach(var pair in topLevelMetadata)
            {
                var nameAndNumber = pair.Value.Split(MetaDataSecondOrderSeperator);

                char symbol = pair.Key[0];
                string templateName = nameAndNumber[0];

                if(Templates.Database.ContainsKey(templateName))
                {
                    int templateNumber = int.Parse(nameAndNumber[1]);
                    var template = Templates.Database[templateName][templateNumber];
                    output[symbol] = template;
                }
            }

            return output;
        }
    }
}
