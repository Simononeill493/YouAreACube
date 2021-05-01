using Newtonsoft.Json;
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
            var template = JsonConvert.DeserializeObject<BlockTemplate>(token.ToString());
            if (template.Speed != 0)
            {
                template.Active = true;
            }

            return template;
        }
    }
}
