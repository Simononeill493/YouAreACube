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
        public static List<BlockTemplate> ParseTemplates(JToken templatesToken)
        {
            var templates = new List<BlockTemplate>();

            foreach (var templateToken in templatesToken)
            {
                var blockTemplate = ParseTemplate(templateToken);
                templates.Add(blockTemplate);
            }

            return templates;
        }
        public static BlockTemplate ParseTemplate(JToken templateToken)
        {
            var template = JsonConvert.DeserializeObject<BlockTemplate>(templateToken.ToString());
            if (template.Speed != 0)
            {
                template.Active = true;
            }

            return template;
        }
    }
}
