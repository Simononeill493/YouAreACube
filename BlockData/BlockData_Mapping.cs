using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public partial class BlockData
    {
        public List<BlockData> InputMappings { get; }

        [JsonIgnore]
        public BlockData BaseMappingBlock { get; set; }
        public string BaseMappingName => BaseMappingBlock == null ? Name : BaseMappingBlock.Name;

        [JsonIgnore]
        public bool IsMappedToSubBlocks { get; private set; }

        private void _initSubMappings()
        {
            if (InputMappings != null)
            {
                IsMappedToSubBlocks = true;
            }

            _inputDataTypeOptions = new List<List<string>>();
            foreach (var inputType in Inputs)
            {
                _inputDataTypeOptions.Add(inputType.Split('|').ToList());
            }
        }

        public List<BlockData> GetAllSubMappings()
        {
            var output = new List<BlockData>();
            if (InputMappings != null)
            {
                foreach (var mapping in InputMappings)
                {
                    output.Add(mapping);
                    output.AddRange(mapping.GetAllSubMappings());
                }
            }

            return output;
        }

        public BlockData GetMappedBlockData(List<string> inputTypes)
        {
            if (IsMappedToSubBlocks)
            {
                return GetFirstMatchingMapping(inputTypes, InputMappings);
            }

            return this;
        }

        public static BlockData GetFirstMatchingMapping(List<string> inputs, List<BlockData> possibleMatches)
        {
            foreach (var possibleMatch in possibleMatches)
            {
                if (DoesMappingMatchInputs(inputs, possibleMatch))
                {
                    return possibleMatch;
                }
            }

            throw new Exception("Blockdata mapping error: block input types do not map to any chip.");
        }

        public static bool DoesMappingMatchInputs(List<string> inputs, BlockData possibleMatch)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                if (!TemplateEditUtils.IsValidInputFor(inputs[i], possibleMatch.Inputs[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
