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
        public const string DemoWorldFilePath = @"C:\Users\Simon\Desktop\Cube\Cube\Simon_Data\DemoWorldFile.txt";

        public const string TopLevelSeperator = "-";
        public const string Level2Seperator = "#";

        public const char TemplateNameVsVersionSeperator = ',';
        public const char SectorSizeXYSeperator = ',';
        public const char TemplateSymbolVsTemplateSeperator = '=';
        public const char WorldPicEmptySpace = ' ';

        public static (IntPoint, List<Sector>, SurfaceCube) LoadDemoSector(WorldKernel kernel)
        {
            var lines = File.ReadAllLines(DemoWorldFilePath).ToList();
            var topLevelSeperated = StringUtils.Seperate(lines, TopLevelSeperator);

            var (sectorSize,topLevelDescs) = ParseWorldMetaData(topLevelSeperated[0]);

            var sectors = topLevelSeperated.GetRange(1, topLevelSeperated.Count - 1);
            var sectorsList = new List<Sector>();

            foreach (var sectorLines in sectors)
            {
                var sector = SectorFromLines(sectorSize, sectorLines, kernel,topLevelDescs);
                sectorsList.Add(sector);
            }

            var player = sectorsList.FirstOrDefault(s => s.AbsoluteLocation.Equals(IntPoint.Zero)).GetPlayer();
            return (sectorSize, sectorsList, player);
        }

        public static (IntPoint sectorSize, Dictionary<char, CubeTemplate> topLevelDescs) ParseWorldMetaData(List<string> lines)
        {
            var worldDataSeperated = StringUtils.Seperate(lines, Level2Seperator);
            var sectorSize = IntPoint.Parse(worldDataSeperated[0][0]);
            var surfaceDescs = DecodeSurfaceDescs(worldDataSeperated[1]);

            return (sectorSize, surfaceDescs);
        }

        private static SurfaceCube GetPlayer(this Sector sector)
        {
            var player = sector.ActiveBlocks.Where(a => a.Template.Name.Equals("BasicPlayer") & a.CubeMode == CubeMode.Surface).FirstOrDefault();
            return (SurfaceCube)player;
        }

        public static Sector SectorFromLines(IntPoint size,List<string> lines,Kernel worldKernel, Dictionary<char, CubeTemplate> topLevelDescs)
        {
            var sections = StringUtils.Seperate(lines,Level2Seperator);
            var location = IntPoint.Parse(sections[0][0]);
            var sectorPic = sections[1];
            var descsLines = sections[2];

            if (sectorPic.Count != size.Y)
            {
                throw new Exception("World picture is the wrong size.");
            }

            var descs = DecodeSurfaceDescs(descsLines);
            foreach(var kvp in topLevelDescs)
            {
                if(!descs.ContainsKey(kvp.Key))
                {
                    descs[kvp.Key] = kvp.Value;
                }
            }

            return MakeSector(sectorPic, size,location, descs,worldKernel);
        }

        public static Sector MakeSector(List<string> sectorPic, IntPoint size,IntPoint location, Dictionary<char, CubeTemplate> descs,Kernel worldKernel)
        {
            var sector = WorldGen.GetEmptyTestSector(location, size);
            var worldGenGrid = new WorldGenGrid(worldKernel, size, new Random());

            for(int i=0; i<size.Y;i++)
            {
                var line = sectorPic[i];
                if (line.Length != size.X) { throw new Exception(); }

                for(int j=0;j<size.X;j++)
                {
                    var tile = line[j];
                    if (tile != WorldPicEmptySpace)
                    {
                        var template = descs[tile];
                        worldGenGrid.TilesGrid[j, i].Surface = template;
                    }
                }
            }

            worldGenGrid.OverlayOnSector(sector);
            WorldGen.FillEnergyBarsForSector(sector);
            return sector;
        }

        public static Dictionary<char,CubeTemplate> DecodeSurfaceDescs(List<string> lines)
        {
            var output = new Dictionary<char, CubeTemplate>();
            foreach(var str in lines)
            {
                var pair = str.Split(TemplateSymbolVsTemplateSeperator);
                var nameAndNumber = pair[1].Split(TemplateNameVsVersionSeperator);

                char symbol = pair[0][0];
                string templateName = nameAndNumber[0];
                int templateNumber = int.Parse(nameAndNumber[1]);

                var template = Templates.Database[templateName][templateNumber];
                output[symbol] = template;
            }

            return output;
        }
    }
}
