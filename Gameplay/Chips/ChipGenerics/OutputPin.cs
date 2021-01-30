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
        public List<InputPin2<TOutputType>> Targets2 = new List<InputPin2<TOutputType>>();

        public abstract void Run(Block actor, UserInput input,EffectsList effects);

        public void SetOutput(TOutputType output)
        {
            foreach(var t in Targets)
            {
                t.ChipInput = output;
            }

            foreach(var t2 in Targets2)
            {
                t2.ChipInput2 = output;
            }
        }
    }
}
