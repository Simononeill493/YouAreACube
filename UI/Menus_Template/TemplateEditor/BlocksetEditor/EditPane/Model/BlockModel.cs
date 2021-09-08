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
        public string OutputType;

        public List<BlockInputModel> Inputs = new List<BlockInputModel>();
        public List<(string, BlocksetModel)> SubBlocksets;

        public BlockModel(string name, BlockData data)
        {
            Name = name;
            ChipName = data.Name;
            OutputType = data.Output;

            Inputs = new List<BlockInputModel>();
            SubBlocksets = new List<(string, BlocksetModel)>();
        }

        public void AddSection(string name, BlocksetModel blockset) => SubBlocksets.Add((name, blockset));

        public BlockData GetVisualBlockData() => BlockDataDatabase.BlockDataDict[ChipName];

        public void MakeBlankInputs()
        {
            for (int i = 0; i < GetVisualBlockData().NumInputs; i++)
            {
                Inputs.Add(new BlockInputModel());
            }
        }
    }
}
