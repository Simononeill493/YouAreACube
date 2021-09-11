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

        public static void TestParsingRoundTrip(string name, ChipsetCollection chipsets, TemplateVariableSet variables)
        {
            foreach (var mode in chipsets.Modes.Values)
            {
                TestParsingRoundTrip(name, mode, variables);
            }
        }
        public static void TestParsingRoundTrip(string name,Chipset chipset,TemplateVariableSet variables)
        {
            var initialJson = chipset.ToJson();

            var blockModel = chipset.ToBlockModel(variables);
            var chipsetFromBlockModel = blockModel.ToChipset();
            var blockModelRoundTripJson = chipsetFromBlockModel.ToJson();

            var chipsetClone = initialJson.ToChipset();
            var chipsetRoundTripJson = chipsetClone.ToJson();

            if (!initialJson.Equals(chipsetRoundTripJson))
            {
                var l = initialJson.Length;
                var m = chipsetRoundTripJson.Length;
                throw new Exception(name + " template parsing mismatch. Lengths are " + l + " and " + m);
            }

            if (!chipsetRoundTripJson.Equals(blockModelRoundTripJson))
            {
                throw new Exception(name + " template parsing mismatch");
            }

            if (!chipset.Equivalent(chipsetClone))
            {
                throw new Exception(name + " template parsing mismatch");
            }

            if (!chipset.Equivalent(chipsetFromBlockModel))
            {
                throw new Exception(name + " template parsing mismatch");
            }
        }
    }
}
