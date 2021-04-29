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

        public ChipBlock()
        {
            Chips = new List<IChip>();
            _controlChips = new List<IControlChip>();
        }
        public ChipBlock(List<IChip> chips)
        {
            Chips = new List<IChip>();
            _controlChips = new List<IControlChip>();

            AddChips(chips);
        }
        public ChipBlock(params IChip[] chips) : this(chips.ToList()) { }

        public void AddChip(IChip chip)
        {
            Chips.Add(chip);
            if(typeof(IControlChip).IsAssignableFrom(chip.GetType()))
            {
                _controlChips.Add((IControlChip)chip);
            }
        }
        public void AddChips(List <IChip> chips)
        {
            Chips.AddRange(chips);
            _controlChips.AddRange(chips.Where(c => typeof(IControlChip).IsAssignableFrom(c.GetType())).Cast<IControlChip>().ToList());
        }

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
            var output = new List<ChipBlock>() { };
            foreach (var controlChip in _controlChips)
            {
                output.AddRange(controlChip.GetSubBlocks());
            }

            return output;
        }

        public bool Equivalent(ChipBlock other)
        {
            if(Name != other.Name) { return false; }
            if (Chips.Count!=other.Chips.Count) { return false; }
            if (_controlChips.Count != other._controlChips.Count) { return false; }

            for (int i = 0; i < Chips.Count; i++)
            {
                if (!Chips[i].Name.Equals(other.Chips[i].Name))
                {
                    return false;
                }
            }

            var mySubBlocks = GetSubBlocks();
            var otherSubBlocks = other.GetSubBlocks();

            if (mySubBlocks.Count != otherSubBlocks.Count) { return false; }

            for (int i=0;i< mySubBlocks.Count;i++)
            {
                if(!mySubBlocks[i].Equivalent(otherSubBlocks[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
