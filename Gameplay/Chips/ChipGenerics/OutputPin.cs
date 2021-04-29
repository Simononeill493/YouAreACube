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
        public string Name { get; set; }

        public List<InputPin<TOutputType>> Targets1 = new List<InputPin<TOutputType>>();
        public List<InputPin2<TOutputType>> Targets2 = new List<InputPin2<TOutputType>>();
        public List<InputPin3<TOutputType>> Targets3 = new List<InputPin3<TOutputType>>();

        public abstract void Run(Block actor, UserInput input,ActionsList actions);

        public void SetOutput(TOutputType output)
        {
            Targets1.ForEach(t => t.ChipInput1 = output);
            Targets2.ForEach(t => t.ChipInput2 = output);
            Targets3.ForEach(t => t.ChipInput3 = output);
        }
    }
}
