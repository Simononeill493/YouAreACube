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

        public List<IChip> Chips;
        private List<IControlChip> _controlChips;

        public ChipBlock(List<IChip> chips)
        {
            Chips = chips;
            _controlChips = chips.Where(c => typeof(IControlChip).IsAssignableFrom(c.GetType())).Cast<IControlChip>().ToList();
        }
        public ChipBlock(params IChip[] chips)
        {
            Chips = new List<IChip>();
            foreach(var chip in chips)
            {
                Chips.Add(chip);
            }
            _controlChips = chips.Where(c => typeof(IControlChip).IsAssignableFrom(c.GetType())).Cast<IControlChip>().ToList();
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
    }
}
