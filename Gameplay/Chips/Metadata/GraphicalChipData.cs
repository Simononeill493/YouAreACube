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
    public class GraphicalChipData
    {
        public string Name { get; }
        public ChipType ChipDataType { get; }
        public string[] Inputs { get; }
        public string Output { get; }
        public List<GraphicalChipData> InputMappings { get; }

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
        public bool IsMappedToSubChips { get; private set; }

        [JsonIgnore]
        public GraphicalChipData BaseMappingChip { get; set; }
        public string BaseMappingName => BaseMappingChip == null ? Name : BaseMappingChip.Name;


        private string[] _inputDisplayNames;
        private List<List<string>> _inputDataTypeOptions;

        public GraphicalChipData(string name, ChipType dataType, string[] inputs, string output, List<GraphicalChipData> inputMappings, string[] inputDisplayNames)
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
            _initIOData();
            _initGenericFlags();
            _initSubMappings();

            if(_inputDisplayNames == null)
            {
                _inputDisplayNames = Inputs;
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
                IsInputGeneric |= Inputs[i].Contains("Variable");
            }

            if (Output != null)
            {
                IsOutputGeneric = Output.Contains("Variable");
            }

            IsGeneric = IsInputGeneric | IsOutputGeneric;
        }
        private void _initSubMappings()
        {
            if(InputMappings!=null)
            {
                IsMappedToSubChips = true;
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
        public List<GraphicalChipData> GetAllSubMappings()
        {
            var output = new List<GraphicalChipData>();
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
    }
}
