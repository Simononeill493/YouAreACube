using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class Chipset
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


        public List<IChip> GetAllChipsAndSubChips()
        {
            var output = new List<IChip>();
            foreach(var block in GetChipsetAndSubChipsets())
            {
                output.AddRange(block.Chips);
            }

            return output;
        }
        public List<Chipset> GetChipsetAndSubChipsets()
        {
            var output = new List<Chipset>() { this };
            output.AddRange(GetSubBlocks());
            return output;
        }
        public List<Chipset> GetSubBlocks()
        {
            var output = new List<Chipset>();
            ControlChips.ForEach(c => output.AddRange(c.GetSubBlocks()));
            return output;
        }


        public static Chipset NoAction = new Chipset() { Name = "No_Action" };
    }
}
