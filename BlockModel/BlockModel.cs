using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockModel
    {
        public string Name;
        public string ChipName;
        private string _baseOutputType;

        public List<BlockInputModel> Inputs = new List<BlockInputModel>();

        [JsonIgnore]
        public List<(string, BlocksetModel)> SubBlocksets;

        public BlockModel(string name, BlockData data)
        {
            Name = name;
            ChipName = data.Name;
            _baseOutputType = data.Output;

            Inputs = new List<BlockInputModel>();
            SubBlocksets = new List<(string, BlocksetModel)>();
        }

        public void AddSection(string name, BlocksetModel blockset) => SubBlocksets.Add((name, blockset));

        public void MakeBlankInputs()
        {
            for (int i = 0; i < GetVisualBlockData().NumInputs; i++)
            {
                Inputs.Add(new BlockInputModel());
            }
        }

        public BlockData GetVisualBlockData() => BlockDataDatabase.BlockDataDict[ChipName].GetThisOrBaseMappingBlock();
        public BlockData GetChipBlockData()
        {
            var visualData = GetVisualBlockData();
            var inputTypes = GetInputTypes();
            var chipBlockData = visualData.GetMappedBlockData(inputTypes);
            return chipBlockData;
        }

        public List<string> GetInputTypes()
        {
            var baseData = GetVisualBlockData();
            var inputTypes = new List<string>();

            for(int i=0;i<baseData.NumInputs;i++)
            {
                var storedType = Inputs[i].StoredType;
                if(storedType==null)
                {
                    storedType = baseData.Inputs[i];
                    if(storedType.Contains("|"))
                    {
                        throw new Exception();
                    }
                }

                inputTypes.Add(storedType);
            }

            return inputTypes;
        }
        public string GetOutputType()
        {
            if(Inputs.Any(i=>i.InputOption.InputOptionType == InputOptionType.Undefined))
            {
                return _baseOutputType;
            }

            if(!GetChipBlockData().IsOutputGeneric)
            {
                return _baseOutputType;
            }

            var typeArguments = GetTypeArguments();
            if(typeArguments.Count==1)
            {
                return typeArguments.First();
            }

            throw new Exception("Block has generic output but multiple type arguments. This could happen in the future but isn't handled.");
        }

        public List<string> GetTypeArguments()
        {
            var data = GetChipBlockData();
            var inputTypes = GetInputTypes();
            var typeArgs = data.GetTypeArguments(inputTypes);

            return typeArgs;
        }

        public List<(string, string)> SubBlocksetsNames => SubBlocksets.Select(s => (s.Item1, s.Item2.Name)).ToList();
    }
}
