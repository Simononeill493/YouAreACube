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
        public static List<CubeTemplate> ParseTemplates(JToken templatesToken)
        {
            var templates = new List<CubeTemplate>();

            foreach (var templateToken in templatesToken)
            {
                var blockTemplate = ParseTemplate(templateToken);
                templates.Add(blockTemplate);
            }

            return templates;
        }

        public static CubeTemplate ParseTemplate(JToken templateToken)
        {
            var template = JsonConvert.DeserializeObject<CubeTemplate>(templateToken.ToString());
            if (template.Speed != 0)
            {
                template.Active = true;
            }
            template.Variables = new TemplateVariableSet();

            return template;
        }
    }
}
