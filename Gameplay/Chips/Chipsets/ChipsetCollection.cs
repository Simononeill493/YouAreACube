using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class ChipsetCollection
    {
        public const string InitialChipName = "_Initial";

        public Dictionary<int, Chipset> Modes;
        public Chipset Initial => Modes[0];

        public ChipsetCollection()
        {
            Modes = new Dictionary<int, Chipset>();
        }
        public ChipsetCollection(Chipset initial) : this()
        {
            initial.Name = InitialChipName;
            Modes[0] = initial;
        }

        public ChipsetCollection Clone(TemplateVariableSet variables)
        {
            var output = new ChipsetCollection();
            foreach(var mode in Modes)
            {
                output.Modes[mode.Key] = mode.Value.ToBlockModel(variables).ToChipset();
            }

            return output;
        }
    }
}
