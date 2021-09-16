using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public partial class Chipset
    {
        public string Name;
        public List<IChip> Chips;
        public List<IControlChip> ControlChips;

        public Chipset(List<IChip> chips)
        {
            Chips = new List<IChip>();
            ControlChips = new List<IControlChip>();

            AddChips(chips);
        }
        public Chipset(params IChip[] chips) : this(chips.ToList()) { }
        public Chipset(string name) : this() { Name = name; }

        public void AddChip(IChip chip)
        {
            Chips.Add(chip);
            if(chip.IsControlChip())
            {
                ControlChips.Add((IControlChip)chip);
            }
        }
        public void AddChips(List <IChip> chips)=>chips.ForEach(c => AddChip(c));
        
        public void Execute(Cube actor, UserInput input, ActionsList actions) => Chips.ForEach(chip => chip.Run(actor, input, actions));


        public List<IControlChip> GetControlChipsCascade() => GetThisAndAllChipsetsCascade().SelectMany(c => c.ControlChips).ToList();
        public List<IChip> GetAllChipsCascade()
        {
            var output = new List<IChip>();
            foreach(var block in GetThisAndAllChipsetsCascade())
            {
                output.AddRange(block.Chips);
            }

            return output;
        }
        public List<Chipset> GetThisAndAllChipsetsCascade()
        {
            var output = new List<Chipset>() { this };
            output.AddRange(GetSubChipsets());
            return output;
        }
        public List<Chipset> GetSubChipsets()
        {
            var output = new List<Chipset>();
            ControlChips.ForEach(c => output.AddRange(c.GetSubChipsetsCascade()));
            return output;
        }


        public static Chipset NoAction = new Chipset() { Name = IDUtils.GenerateBlocksetID() };
    }
}
