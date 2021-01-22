﻿using Newtonsoft.Json;
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
        private static Dictionary<string, BlockTemplate> _blocks;

        public static void Load() 
        {
            _blocks = new Dictionary<string, BlockTemplate>();

            var data = File.ReadAllText(Config.TemplatesFile);
            JObject result = (JObject)JsonConvert.DeserializeObject(data);
            var blocks = result["blocks"];
            foreach(var token in blocks)
            {
                var blockTemplate = ParseBlock(token);
                _blocks[blockTemplate.Name] = blockTemplate;
            }

            _blocks["BasicEnemy"].Chips = ChipMaker.TestEnemyBlock;
            _blocks["ScaredEnemy"].Chips = ChipMaker.TestFleeBlock;
        }

        private static BlockTemplate ParseBlock(JToken token)
        {
            var name = token["name"].ToString();
            var template = new BlockTemplate(name);
            template.Sprite = token["sprite"].ToString();
            template.Speed = int.Parse(token["speed"].ToString());

            if (template.Speed != 0)
            {
                template.Active = true;
            }

            return template;
        }

        public static Block Generate(string name)
        {
            return _blocks[name].Generate();
        }
    }
}
