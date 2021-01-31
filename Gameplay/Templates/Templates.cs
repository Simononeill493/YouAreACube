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

            BlockTemplates = _parseBlocks(data["blocks"]);

            //todo this is temporary
            BlockTemplates["BasicEnemy"].Chips = ChipTester.TestFleeBlock;
            BlockTemplates["ScaredEnemy"].Chips = ChipTester.TestFleeBlock;
            BlockTemplates["Spinner"].Chips = ChipTester.TestSpinBlock;
            BlockTemplates["Bullet"].Chips = ChipTester.TestSpinBlock;

            BlockTemplates["BasicPlayer"].Chips = ChipTester.TestPlayerBlock;
        }

        private static Dictionary<string,BlockTemplate> _parseBlocks(JToken input)
        {
            var blocksDict = new Dictionary<string, BlockTemplate>(); 

            foreach (var token in input)
            {
                var blockTemplate = _parseBlock(token);
                blocksDict[blockTemplate.Name] = blockTemplate;
            }

            return blocksDict;
        }
        private static BlockTemplate _parseBlock(JToken token)
        {
            var name = token["name"].ToString();
            var sprite = token["sprite"].ToString();
            var speed = int.Parse(token["speed"].ToString());
            var energyCap = token["energyCap"] == null ? 30 : int.Parse(token["energyCap"].ToString());

            var template = new BlockTemplate(name);

            template.Sprite = sprite;
            template.Speed = speed;
            template.EnergyCap = energyCap;

            if (template.Speed != 0)
            {
                template.Active = true;
            }

            return template;
        }

        public static Block Generate(string name,BlockType blockType) => BlockTemplates[name].Generate(blockType);
        public static SurfaceBlock GenerateSurface(string name) => BlockTemplates[name].GenerateSurface();
        public static GroundBlock GenerateGround(string name) => BlockTemplates[name].GenerateGround();
        public static EphemeralBlock GenerateEphemeral(string name) => BlockTemplates[name].GenerateEphemeral();
    }
}
