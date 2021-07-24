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
                TestParsingRoundTrip(template.Name, template.Chipset);
            }
        }

        public static void TestParsingRoundTrip(string name,Chipset chipset)
        {
            var initialJson = Parser_ChipsetToJSON.ParseChipsetToJson(chipset);

            var editableChipset = Parser_JSONToEditableChipset.ParseJsonToBlockset(initialJson, new DummyBlocksetGenerator());
            var chipBlockClone = Parser_JSONToChipset.ParseJsonToBlock(initialJson);

            var chipsetRoundTripJson = Parser_BlockSetToJSON.ParseEditableChipsetToJson(editableChipset);
            var chipBlockRoundTripJson = Parser_ChipsetToJSON.ParseChipsetToJson(chipBlockClone);

            if (!initialJson.Equals(chipsetRoundTripJson))
            {
                var l = initialJson.Length;
                var m = chipsetRoundTripJson.Length;
                throw new Exception(name + " template parsing mismatch");
            }

            if (!chipsetRoundTripJson.Equals(chipBlockRoundTripJson))
            {
                throw new Exception(name + " template parsing mismatch");
            }

            if (!chipset.Equivalent(chipBlockClone))
            {
                throw new Exception(name + " template parsing mismatch");
            }
        }
    }
}
