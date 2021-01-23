using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public abstract class OutputPin<TOutputType> : IChip
    {
        public List<InputPin<TOutputType>> Targets = new List<InputPin<TOutputType>>();

        public abstract void Run(Block actor, UserInput input);

        public void SetOutput(TOutputType output)
        {
            foreach(var t in Targets)
            {
                t.ChipInput = output;
            }
        }
    }
}
