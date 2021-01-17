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
        public static GroundBlock VoidBlock;

        private static Dictionary<string, BlockTemplate> _blocks;

        public static void Load() 
        {
            _blocks = new Dictionary<string, BlockTemplate>();

            VoidBlock = new GroundBlock() { Sprite = "Black" };

            var data = File.ReadAllText(Config.TemplatesFile);
            JObject result = (JObject)JsonConvert.DeserializeObject(data);
            var blocks = result["blocks"];
            foreach(var token in blocks)
            {
                var blockTemplate = ParseBlock(token);
                _blocks[blockTemplate.Name] = blockTemplate;
            }
        }

        private static BlockTemplate ParseBlock(JToken token)
        {
            var block = new Block();
            block.Sprite = token["sprite"].ToString();

            var name = token["name"].ToString();
            return new BlockTemplate(name,block);
        }

        public static Block Generate(string name)
        {
            return _blocks[name].Generate();
        }
    }
}
