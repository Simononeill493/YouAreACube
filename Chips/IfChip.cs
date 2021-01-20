using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class IfChip
    {
        public IEvaluatedChip ToEvaluate;
        public IChip Yes;
        public IChip No;

        public IfChip(IEvaluatedChip toEvaluate, IChip yes, IChip no)
        {
            ToEvaluate = toEvaluate;
            Yes = yes;
            No = no;
        }

        public bool Process(Block block, UserInput userInput)
        {
            if(ToEvaluate.Evaluate(block,userInput))
            {
                return Yes.Process(block, userInput);
            }

            return No.Process(block, userInput);
        }
    }
}
