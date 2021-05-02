using Microsoft.Xna.Framework;
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

        public int NumInputs { get; private set; }
        public bool HasOutput { get; private set; }

        public bool IsGeneric { get; private set; }
        public bool IsInputGeneric { get; private set; }
        public bool IsOutputGeneric { get; private set; }

        public GraphicalChipData(string name, ChipType dataType, string[] inputs, string output)
        {
            Name = name;
            ChipDataType = dataType;
            Inputs = inputs;
            Output = output;
        }

        public void Init()
        {
            if(Inputs!=null)
            {
                NumInputs = Inputs.Count();
            }
            HasOutput = Output != null;

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

        public Color ChipColor => ChipDataType.GetColor();
        public string GetInputType(int num) => Inputs[num];
        public override string ToString() => Name + " (" + ChipDataType.ToString() + ")";
    }
}
