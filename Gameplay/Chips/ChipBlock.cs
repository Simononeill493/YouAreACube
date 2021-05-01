using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class ChipBlock
    {
        public string Name;
        public List<IChip> Chips;
        public List<IControlChip> ControlChips;

        public ChipBlock(List<IChip> chips)
        {
            Chips = new List<IChip>();
            ControlChips = new List<IControlChip>();

            AddChips(chips);
        }
        public ChipBlock(params IChip[] chips) : this(chips.ToList()) { }

        public void AddChip(IChip chip)
        {
            Chips.Add(chip);
            if(chip.IsControlChip())
            {
                ControlChips.Add((IControlChip)chip);
            }
        }
        public void AddChips(List <IChip> chips)=>chips.ForEach(c => AddChip(c));
        public void Execute(Block actor, UserInput input, ActionsList actions) => Chips.ForEach(chip => chip.Run(actor, input, actions));


        public List<IChip> GetAllChipsAndSubChips()
        {
            var output = new List<IChip>();
            foreach(var block in GetBlockAndSubBlocks())
            {
                output.AddRange(block.Chips);
            }

            return output;
        }
        public List<ChipBlock> GetBlockAndSubBlocks()
        {
            var output = new List<ChipBlock>() { this };
            output.AddRange(GetSubBlocks());
            return output;
        }
        public List<ChipBlock> GetSubBlocks()
        {
            var output = new List<ChipBlock>();
            ControlChips.ForEach(c => output.AddRange(c.GetSubBlocks()));
            return output;
        }


        public static ChipBlock NoAction = new ChipBlock();
    }
}
