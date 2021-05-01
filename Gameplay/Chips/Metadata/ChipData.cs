using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipData
    {
        public string Name { get; }
        public ChipType ChipDataType { get; }

        private string[] Inputs { get; }
        public int NumInputs { get; }

        public string Output { get; }
        public bool HasOutput { get; }

        public bool IsGeneric { get; private set; }
        public bool IsInputGeneric { get; private set; }
        public bool IsOutputGeneric { get; private set; }


        public ChipData(string name,ChipType chipType, int numInputs, string[] inputs, string output)
        {
            Name = name;
            ChipDataType = chipType;

            Inputs = inputs;
            NumInputs = numInputs;

            Output = output;
            HasOutput = Output != null;

            _setGenericFlags();
        }

        private void _setGenericFlags()
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

        public Color ChipColor => ChipDataType.GetColor();
        public string GetInputType(int num) => Inputs[num];
        public override string ToString() => Name + " (" + ChipDataType.ToString() + ")";
    }
}
