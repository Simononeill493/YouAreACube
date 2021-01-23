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
        public List<IChip> Chips;
        private List<IControlChip> _controlChips;

        public ChipBlock(List<IChip> chips)
        {
            Chips = chips;
            _controlChips = chips.Where(c => typeof(IControlChip).IsAssignableFrom(c.GetType())).Cast<IControlChip>().ToList();
        }

        public void Execute(Block actor,UserInput input) 
        { 
            foreach(var chip in Chips)
            {
                chip.Run(actor,input);
            }

            foreach(var controlChip in _controlChips)
            {
                controlChip.Result.Execute(actor, input);
            }
        }
    }
}
