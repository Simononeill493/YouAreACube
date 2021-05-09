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
        public static void TestParsingRoundTrips(List<BlockTemplate> templates)
        {
            foreach (var template in templates)
            {
                if (!template.Active) { continue; }
                TestParsingRoundTrip(template.Name, template.ChipBlock);
            }
        }

        public static void TestParsingRoundTrip(string name,ChipBlock chipBlock)
        {
            var initialJson = ChipBlockToJSONParser.ParseBlockToJson(chipBlock);

            var editableChipset = JSONToEditableChipsetParser.ParseJsonToEditableChipset(initialJson, new DummyChipsetGenerator());
            var chipBlockClone = JSONToChipBlockParser.ParseJsonToBlock(initialJson);

            var chipsetRoundTripJson = EditableChipsetToJSONParser.ParseEditableChipsetToJson(editableChipset);
            var chipBlockRoundTripJson = ChipBlockToJSONParser.ParseBlockToJson(chipBlockClone);

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

            if (!ChipBlockComparer.Equivalent(chipBlock, chipBlockClone))
            {
                throw new Exception(name + " template parsing mismatch");
            }
        }
    }
}
