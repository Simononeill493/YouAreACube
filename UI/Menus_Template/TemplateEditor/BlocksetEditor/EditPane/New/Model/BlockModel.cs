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
        public string TypeName;

        public string OutputTypeName;

        public List<BlockInputModel> Inputs = new List<BlockInputModel>();

        public BlockModel(string name,BlockData data)
        {
            Name = name;
            TypeName = data.Name;
            OutputTypeName = data.Output;
           
            Inputs = new List<BlockInputModel>();
            for(int i=0;i<data.NumInputs;i++)
            {
                Inputs.Add(new BlockInputModel());
            }
        }
    }
}
