using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateParsingTester
    {
        public static void TestParsingRoundTrips(TemplateDatabase templates) => TestParsingRoundTrips(templates.GetAllVersionsOfAllTemplates());
        public static void TestParsingRoundTrips(List<CubeTemplate> templates)
        {
            foreach (var template in templates)
            {
                if (!template.Active) { continue; }
                if(template.Chipsets!=null)
                {
                    TestParsingRoundTrip(template.Name, template.Chipsets, template.Variables);
                }
            }
        }

        public static void TestParsingRoundTrip(string name, ChipsetCollection chipset,TemplateVariableSet variables)
        {
            var blockModel = chipset.ToBlockModel(variables);
            var chipsetFromBlockModel = blockModel.ToChipsets();

            if (!chipset.Equivalent(chipsetFromBlockModel))
            {
                throw new Exception(name + " template parsing mismatch");
            }
        }

        public static void TestParsingRoundTrip(string name, FullModel model,TemplateVariableSet variables)
        {
            var chipset = model.ToChipsets();
            var newModel = chipset.ToBlockModel(variables);

            var blockJson = JToken.FromObject(model, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore }).ToString();
            var newJson = JToken.FromObject(newModel, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore }).ToString();

            if (!blockJson.Equals(newJson))
            {
                var l1 = blockJson.Length;
                var l2 = newJson.Length;

                throw new Exception(name + " template parsing mismatch");
            }
        }
    }
}
