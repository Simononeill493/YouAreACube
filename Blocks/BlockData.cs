using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class BlockData
    {
        public string Name { get; }
        public ChipType ChipDataType { get; }
        public string[] Inputs { get; }
        public string Output { get; }
        public List<BlockData> InputMappings { get; }

        [JsonIgnore]
        public int NumInputs { get; private set; }
        [JsonIgnore]
        public bool HasOutput { get; private set; }
        [JsonIgnore]
        public bool IsGeneric { get; private set; }
        [JsonIgnore]
        public bool IsInputGeneric { get; private set; }
        [JsonIgnore]
        public bool IsOutputGeneric { get; private set; }
        [JsonIgnore]
        public bool IsMappedToSubBlocks { get; private set; }

        [JsonIgnore]
        public List<string> DefaultTypeArguments;
        [JsonIgnore]
        public BlockData BaseMappingBlock { get; set; }
        public string BaseMappingName => BaseMappingBlock == null ? Name : BaseMappingBlock.Name;


        private string[] _inputDisplayNames;
        private List<List<string>> _inputDataTypeOptions;

        public BlockData(string name, ChipType dataType, string[] inputs, string output, List<BlockData> inputMappings, string[] inputDisplayNames)
        {
            Name = name;
            ChipDataType = dataType;
            Inputs = inputs;
            Output = output;
            InputMappings = inputMappings;

            _inputDisplayNames = inputDisplayNames;
        }

        public void Init()
        {
            DefaultTypeArguments = new List<string>();

            _initIOData();
            _initGenericFlags();
            _initSubMappings();

            if(_inputDisplayNames == null)
            {
                _inputDisplayNames = Inputs;
            }

            if(!IsGeneric)
            {
                DefaultTypeArguments = null;
            }
        }
        private void _initIOData()
        {
            if (Inputs != null)
            {
                NumInputs = Inputs.Count();
            }
            HasOutput = Output != null;
        }
        private void _initGenericFlags()
        {
            for (int i = 0; i < NumInputs; i++)
            {
                IsInputGeneric |= _checkIsGeneric(Inputs[i]);
            }

            if (Output != null)
            {
                IsOutputGeneric = _checkIsGeneric(Output);
            }

            IsGeneric = IsInputGeneric | IsOutputGeneric;
        }
        private bool _checkIsGeneric(string typeName)
        {
            if(typeName.Contains("Variable"))
            {
                if(!DefaultTypeArguments.Contains("Object"))
                {
                    DefaultTypeArguments.Add("Object");
                }
                return true;
            }
            if (typeName.Contains("AnyCube"))
            {
                if (!DefaultTypeArguments.Contains("SurfaceCube"))
                {
                    DefaultTypeArguments.Add("SurfaceCube");
                }
                return true;
            }

            return false;
        }
        private void _initSubMappings()
        {
            if(InputMappings!=null)
            {
                IsMappedToSubBlocks = true;
            }

            _inputDataTypeOptions = new List<List<string>>();
            foreach (var inputType in Inputs)
            {
                _inputDataTypeOptions.Add(inputType.Split('|').ToList());
            }

        }

        public string GetInputType(int index) => Inputs[index];
        public List<string> GetInputTypes(int index) => _inputDataTypeOptions[index];
        public string GetInputDisplayName(int index)
        {
            return _inputDisplayNames[index];
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

        public override string ToString() => Name + " (" + ChipDataType.ToString() + ")";

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
