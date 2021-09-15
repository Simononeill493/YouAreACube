using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public partial class ChipsetCollection
    {
        public Dictionary<int, Chipset> Modes;
        public Chipset Initial => Modes[0];

        public ChipsetCollection()
        {
            Modes = new Dictionary<int, Chipset>();
        }
        public ChipsetCollection(Chipset initial) : this()
        {
            Modes[0] = initial;
        }

        public ChipsetCollection Clone(TemplateVariableSet variables)
        {
            var output = this.ToBlockModel(variables).ToChipsets();
            return output;
        }

        public IEnumerable<Chipset> GetAllChipsets() => Modes.Values.SelectMany(c => c.GetThisAndAllChipsetsCascade());
        public IEnumerable<IChip> GetAllChips() => Modes.Values.SelectMany(c => c.GetAllChipsCascade());
        public IEnumerable<IControlChip> GetAllControlChips() => Modes.Values.SelectMany(c => c.GetControlChipsCascade());
        public bool HasMode(Chipset chipset) => Modes.ContainsValue(chipset);

    }
}
