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

        public static (Sector,SurfaceCube) LoadDemoSector(WorldKernel kernel)
        {
            var lines = File.ReadAllLines(DemoWorldFilePath).ToList();
            var sector = FromLines(lines, kernel);
            var player = sector.GetPlayer();
            return (sector,player);
        }

        private static SurfaceCube GetPlayer(this Sector sector)
        {
            var player = sector.ActiveBlocks.Where(a => a.Template.Name.Equals("BasicPlayer") & a.CubeMode == CubeMode.Surface).FirstOrDefault();
            return (SurfaceCube)player;
        }

        public static Sector FromLines(List<string> lines,Kernel worldKernel)
        {
            var sectorSizeStr = lines[0].Split(',');
            var sectorSize = new IntPoint(int.Parse(sectorSizeStr[0]), int.Parse(sectorSizeStr[1]));

            var endOfSectorPic = lines.IndexOf("#");
            if(endOfSectorPic!=sectorSize.Y+1)
            {
                throw new Exception("World picture is the wrong size.");
            }

            var sectorPic = lines.GetRange(1, sectorSize.Y);
            var surfaceDescs = lines.GetRange(endOfSectorPic+1, lines.Count - endOfSectorPic - 1);
            var descs = DecodeSurfaceDescs(surfaceDescs);

            return MakeSector(sectorPic,sectorSize,descs,worldKernel);
        }

        public static Sector MakeSector(List<string> sectorPic, IntPoint size, Dictionary<char, CubeTemplate> descs,Kernel worldKernel)
        {
            var sector = WorldGen.GetEmptyTestSector(IntPoint.Zero, size);
            var worldGenGrid = new WorldGenGrid(worldKernel, size, new Random());

            for(int i=0; i<size.Y;i++)
            {
                var line = sectorPic[i];
                if (line.Length != size.X) { throw new Exception(); }

                for(int j=0;j<size.X;j++)
                {
                    var tile = line[j];
                    if (tile != ' ')
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
                var pair = str.Split('=');
                var nameAndNumber = pair[1].Split(',');

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
