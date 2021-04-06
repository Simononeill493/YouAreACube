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

            foreach(var template in BlockTemplates.ToList())
            {
                if(!template.Value.Active) { continue; }
                var json1 = ChipBlockParser.ParseBlockToJson(template.Value.Chips);

                var block1 = ChipBlockParser.ParseJsonToBlock(json1);
                var json2 = ChipBlockParser.ParseBlockToJson(block1);

                var chipset1 = EditableChipsetParser.ParseJsonToEditableChipset(json2, new DummyChipsetGenerator());
                var json3 = EditableChipsetParser.ParseEditableChipsetToJson(chipset1);

                if (!json1.Equals(json2))
                {
                    throw new Exception("Parsing Problem - string round trip mismatch");
                }

                if (!json1.Equals(json3))
                {
                    //throw new Exception("Parsing Problem - string round trip mismatch");
                }


                if (!template.Value.Chips.Equivalent(block1))
                {
                    throw new Exception("Parsing Problem - chipblock round trip mismatch");
                }

                BlockTemplates[template.Key].Chips = block1;
            }
        }

        public static Block Generate(string name,BlockMode blockType) => BlockTemplates[name].Generate(blockType);
        public static SurfaceBlock GenerateSurface(string name) => BlockTemplates[name].GenerateSurface();
        public static GroundBlock GenerateGround(string name) => BlockTemplates[name].GenerateGround();
        public static EphemeralBlock GenerateEphemeral(string name) => BlockTemplates[name].GenerateEphemeral();
    }
}
