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
        public string OutputTypeCurrent { get; private set; }

        public void SetOutputType(string outputType)
        {
            OutputType = outputType;
            OutputTypeCurrent = outputType;
        }

        public bool HasOutput;
        public bool IsGeneric;
        public bool OutputIsGeneric => OutputType.Contains("Variable");

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

        public string IsInputGeneric(int num)
        {
            if (num == 1) { return Input1; }
            if (num == 2) { return Input2; }
            if (num == 3) { return Input3; }
            return "_null_";
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

        public override string ToString()
        {
            return Name + " (" + ChipDataType.ToString() + ")";
        }

        public (bool canFeed,bool isGeneric,string baseOfVariable) CanFeedOutputInto(string inputType)
        {
            if(OutputTypeCurrent == null)
            {
                return (false, false, "");
            }

            if (inputType.Equals(OutputTypeCurrent))
            {
                return (true, false,OutputTypeCurrent);
            }

            if(inputType.Equals("Variable"))
            {
                return (true, true,OutputTypeCurrent);
            }

            if(inputType.Equals("List<Variable>"))
            {
                if(OutputTypeCurrent.StartsWith("List<") & OutputTypeCurrent.EndsWith(">"))
                {
                    var afterOpeningList = OutputTypeCurrent.Substring(5);
                    var baseOutput = afterOpeningList.Substring(0, afterOpeningList.Length - 1);
                    return (true,true,baseOutput);
                }
            }

            return (false,false,"");
        }

        public void SetOutputTypeFromGeneric(string actualType)
        {
            if(OutputType!=null)
            {
                OutputTypeCurrent = OutputType.Replace("Variable", actualType);
            }
        }
        public void ResetOutputType()
        {
            OutputTypeCurrent = OutputType;
        }

    }
}
