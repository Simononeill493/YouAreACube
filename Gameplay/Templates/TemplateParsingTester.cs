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
                var initialJson = ChipBlockToJSONParser.ParseBlockToJson(template.ChipBlock);

                var editableChipset = JSONToEditableChipsetParser.ParseJsonToEditableChipset(initialJson, new DummyChipsetGenerator());
                var chipBlockClone = JSONToChipBlockParser.ParseJsonToBlock(initialJson);

                var chipsetRoundTripJson = EditableChipsetToJSONParser.ParseEditableChipsetToJson(editableChipset);
                var chipBlockRoundTripJson = ChipBlockToJSONParser.ParseBlockToJson(chipBlockClone);

                if (!initialJson.Equals(chipsetRoundTripJson))
                {
                    throw new Exception(template.Name + " template parsing mismatch");
                }

                if (!chipsetRoundTripJson.Equals(chipBlockRoundTripJson))
                {
                    throw new Exception(template.Name + " template parsing mismatch");
                }

                if (!ChipBlockComparer.Equivalent(template.ChipBlock, chipBlockClone))
                {
                    throw new Exception(template.Name + " template parsing mismatch");
                }
            }
        }
    }
}
