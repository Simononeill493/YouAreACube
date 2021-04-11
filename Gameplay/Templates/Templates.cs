using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public partial class Templates
    {
        public static TemplatesDatabase BlockTemplates;

        public static void Load() 
        {
            BlockTemplates = new TemplatesDatabase();
            var data = FileUtils.LoadJson(Config.TemplatesFilePath);

            var builtInTemplates = TemplateParser.ParseTemplates(data["blocks"]);
            foreach(var builtInTemplate in builtInTemplates)
            {
                var versions = new TemplateVersionList(builtInTemplate.Key);
                versions[0] = builtInTemplate.Value;
                BlockTemplates[builtInTemplate.Key] = versions;
            }

            //todo this is temporary
            BlockTemplates["BasicEnemy"][0].Chips = ChipTester.TestEnemyBlock;
            BlockTemplates["ScaredEnemy"][0].Chips = ChipTester.TestFleeBlock;
            BlockTemplates["Spinner"][0].Chips = ChipTester.TestSpinBlock;
            BlockTemplates["Bullet"][0].Chips = ChipTester.TestBulletBlock;
            BlockTemplates["MiniBullet"][0].Chips = ChipTester.TestBulletBlock;
            BlockTemplates["BasicPlayer"][0].Chips = ChipTester.TestPlayerBlock;

            var bullet2 = BlockTemplates["Bullet"][0].Clone();
            bullet2.Chips = ChipTester.TestBulletV2Block;
            BlockTemplates["Bullet"][1] = bullet2;

            _testTemplateParsing();
        }

        private static void _testTemplateParsing()
        {
            foreach (var template in BlockTemplates.GetAllVersionsOfAllTemplates())
            {
                if (!template.Active) { continue; }
                var initialJson = ChipBlockParser.ParseBlockToJson(template.Chips);

                var chipset = EditableChipsetParser.ParseJsonToEditableChipset(initialJson, new DummyChipsetGenerator());
                var chipBlock = ChipBlockParser.ParseJsonToBlock(initialJson);

                var chipsetRoundTrip = EditableChipsetParser.ParseEditableChipsetToJson(chipset);
                var blockRoundTrip = ChipBlockParser.ParseBlockToJson(chipBlock);

                if (!chipsetRoundTrip.Equals(blockRoundTrip))
                {
                    throw new Exception();
                }

                if (!template.Chips.Equivalent(chipBlock))
                {
                    throw new Exception();
                }
            }
        }

        public static Block Generate(string name,int version,BlockMode blockType ) => BlockTemplates[name][version].Generate(blockType);
        public static SurfaceBlock GenerateSurface(string name, int version) => BlockTemplates[name][version].GenerateSurface();
        public static GroundBlock GenerateGround(string name, int version) => BlockTemplates[name][version].GenerateGround();
        public static EphemeralBlock GenerateEphemeral(string name, int version) => BlockTemplates[name][version].GenerateEphemeral();
    }
}
