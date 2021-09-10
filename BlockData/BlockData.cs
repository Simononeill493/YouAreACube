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
    public partial class BlockData
    {
        public string Name { get; }
        public ChipType ChipDataType { get; }
        public string[] Inputs { get; }
        public string Output { get; }

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
        public List<string> DefaultTypeArguments;


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
                IsInputGeneric |= InGameTypeUtils.IsGeneric(Inputs[i]);
                _setDefaultTypeArgs(Inputs[i]);
            }

            if (Output != null)
            {
                IsOutputGeneric = InGameTypeUtils.IsGeneric(Output);
                _setDefaultTypeArgs(Output);
            }

            IsGeneric = IsInputGeneric | IsOutputGeneric;
        }

        private void _setDefaultTypeArgs(string typeName)
        {
            var defaultArg = InGameTypeUtils.GetDefaultTypeArgument(typeName);
            if(defaultArg!=null)
            {
                if (!DefaultTypeArguments.Contains(defaultArg))
                {
                    DefaultTypeArguments.Add(defaultArg);
                }
            }
        }

        public List<string> GetTypeArguments(List<string> selectedTypes)
        {
            if(selectedTypes.Count!= NumInputs) { throw new Exception(); }
            var argus = new List<string>();

            for(int i=0;i<NumInputs;i++)
            {
                if(InGameTypeUtils.IsGeneric(Inputs[i]))
                {
                    argus.Add(InGameTypeUtils.GetTypeArgument(Inputs[i],selectedTypes[i]));
                }
            }

            return argus;
        }

        public string GetInputType(int index) => Inputs[index];
        public List<string> GetInputTypes(int index) => _inputDataTypeOptions[index];
        public string GetInputDisplayName(int index)
        {
            return _inputDisplayNames[index];
        }


        public override string ToString() => Name + " (" + ChipDataType.ToString() + ")";
        public Color GetColor() => ChipDataType.GetColor();

    }
}
