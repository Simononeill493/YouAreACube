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

        public List<string> GetInputTypes() => Inputs.Select(i => i.StoredType).ToList();
        public List<string> GetTypeArguments()
        {
            var data = GetChipBlockData();
            var typeArgs = data.GetTypeArguments(GetInputTypes());

            return typeArgs;
        }

        public string GetCurrentOutputType()
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
    }
}
