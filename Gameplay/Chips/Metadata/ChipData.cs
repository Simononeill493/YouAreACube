using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipData
    {
        public string Name;
        public string NameLower;

        public ChipType ChipDataType;

        public string Input1;
        public string Input2;
        public string Input3;

        public string OutputType { get; private set; }
        public void SetOutputType(string outputType)
        {
            OutputType = outputType;
            OutputIsGeneric = outputType.Contains("Variable");
            HasOutput = true;
        }

        public bool HasOutput;
        public bool IsGeneric;
        public bool OutputIsGeneric;

        public int NumInputs;

        public ChipData(string name,ChipType chipType)
        {
            Name = name;
            NameLower = name.ToLower();

            ChipDataType = chipType;
        }

        public Color ChipColor => this.ChipDataType.GetColor();

        public string GetInputType(int num)
        {
            if (num == 1) { return Input1; }
            if (num == 2) { return Input2; }
            if (num == 3) { return Input3; }
            return "_null_";
        }

        public bool IsInputGeneric(int num)
        {
            if(num>NumInputs) { return false; }

            if (num == 1) { return Input1.Contains("Variable"); }
            if (num == 2) { return Input2.Contains("Variable"); }
            if (num == 3) { return Input3.Contains("Variable"); }
            throw new Exception();
        }

        public void Init()
        {
            IsGeneric |= (Input1 == null) ? false : Input1.Contains("Variable");
            IsGeneric |= (Input2 == null) ? false : Input2.Contains("Variable");
            IsGeneric |= (Input3 == null) ? false : Input3.Contains("Variable");
            IsGeneric |= (OutputType == null) ? false : OutputType.Contains("Variable");

            if (Input1 != null) { NumInputs++; }
            if (Input2 != null) { NumInputs++; }
            if (Input3 != null) { NumInputs++; }
        }

        public override string ToString() => Name + " (" + ChipDataType.ToString() + ")";
    }
}
