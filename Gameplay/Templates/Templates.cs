using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Templates
    {
        public static Dictionary<string, BlockTemplate> BlockTemplates;

        public static void Load() 
        {
            var data = FileUtils.LoadJson(Config.TemplatesFile);

            BlockTemplates = TemplateParser.ParseTemplates(data["blocks"]);

            //todo this is temporary
            BlockTemplates["BasicEnemy"].Chips = ChipTester.TestEnemyBlock;
            BlockTemplates["ScaredEnemy"].Chips = ChipTester.TestFleeBlock;
            BlockTemplates["Spinner"].Chips = ChipTester.TestSpinBlock;
            BlockTemplates["Bullet"].Chips = ChipTester.TestBulletBlock;
            BlockTemplates["BasicPlayer"].Chips = ChipTester.TestPlayerBlock;

            _testTemplateParsing();
        }

        private static void _testTemplateParsing()
        {
            foreach (var template in BlockTemplates.ToList())
            {
                if (!template.Value.Active) { continue; }
                var initialJson = ChipBlockParser.ParseBlockToJson(template.Value.Chips);

                var chipset = EditableChipsetParser.ParseJsonToEditableChipset(initialJson, new DummyChipsetGenerator());
                var chipBlock = ChipBlockParser.ParseJsonToBlock(initialJson);

                var chipsetRoundTrip = EditableChipsetParser.ParseEditableChipsetToJson(chipset);
                var blockRoundTrip = ChipBlockParser.ParseBlockToJson(chipBlock);

                if (!chipsetRoundTrip.Equals(blockRoundTrip))
                {
                    throw new Exception();
                }

                if (!template.Value.Chips.Equivalent(chipBlock))
                {
                    throw new Exception();
                }
            }
        }

        public static Block Generate(string name,BlockMode blockType) => BlockTemplates[name].Generate(blockType);
        public static SurfaceBlock GenerateSurface(string name) => BlockTemplates[name].GenerateSurface();
        public static GroundBlock GenerateGround(string name) => BlockTemplates[name].GenerateGround();
        public static EphemeralBlock GenerateEphemeral(string name) => BlockTemplates[name].GenerateEphemeral();
    }
}
