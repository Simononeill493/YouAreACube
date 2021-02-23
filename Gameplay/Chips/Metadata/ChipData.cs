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
        public ChipType ChipType;

        public string Input1;
        public string Input2;
        public string Input3;

        public string Output;

        public bool IsGeneric;

        public ChipData(string name,ChipType chipType)
        {
            Name = name;
            ChipType = chipType;
        }

        public void Init()
        {
            IsGeneric |= (Input1 == null) ? false : Input1.Contains("Variable");
            IsGeneric |= (Input2 == null) ? false : Input2.Contains("Variable");
            IsGeneric |= (Input3 == null) ? false : Input3.Contains("Variable");
            IsGeneric |= (Output == null) ? false : Output.Contains("Variable");
        }

        public override string ToString()
        {
            return Name + " (" + ChipType.ToString() + ")";
        }
    }
}
