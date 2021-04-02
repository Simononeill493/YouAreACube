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
        public static ChipBlock NoAction = new ChipBlock(new List<IChip>());

        public string Name;

        public List<IChip> Chips;
        private List<IControlChip> _controlChips;

        public ChipBlock(List<IChip> chips)
        {
            Chips = new List<IChip>();
            _controlChips = new List<IControlChip>();

            AddChips(chips);
        }
        public ChipBlock(params IChip[] chips) : this(chips.ToList()) { }

        public void AddChip(IChip chip) => AddChips(new List<IChip>() { chip });
        public void AddChips(List <IChip> chips)
        {
            foreach (var chip in chips)
            {
                Chips.Add(chip);
            }
            _controlChips.AddRange(chips.Where(c => typeof(IControlChip).IsAssignableFrom(c.GetType())).Cast<IControlChip>().ToList());
        }

        public void Execute(Block actor,UserInput input,ActionsList actions) 
        { 
            foreach(var chip in Chips)
            {
                chip.Run(actor,input,actions);
            }

            foreach(var controlChip in _controlChips)
            {
                controlChip.ExecuteOutput(actor, input,actions);
            }
        }

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

            foreach (var controlChip in _controlChips)
            {
                output.AddRange(controlChip.GetSubBlocks());
            }

            return output;
        }
    }
}
