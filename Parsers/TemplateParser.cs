using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateParser
    {
        public static Dictionary<string, BlockTemplate> ParseTemplates(JToken input)
        {
            var blocksDict = new Dictionary<string, BlockTemplate>();

            foreach (var token in input)
            {
                var blockTemplate = ParseTemplate(token);
                blocksDict[blockTemplate.Name] = blockTemplate;
            }

            return blocksDict;
        }
        public static BlockTemplate ParseTemplate(JToken token)
        {
            var name = token["name"].ToString();
            var sprite = token["sprite"].ToString();
            var speed = int.Parse(token["speed"].ToString());
            var energyCap = token["energyCap"] == null ? 30 : int.Parse(token["energyCap"].ToString());

            var template = new BlockTemplate(name);

            var version = token["version"];
            if (version != null)
            {
                template.Version = int.Parse(version.ToString());
            }

            template.Sprite = sprite;
            template.Speed = speed;
            template.InitialEnergy = energyCap;

            if (template.Speed != 0)
            {
                template.Active = true;
            }

            return template;
        }
    }
}
