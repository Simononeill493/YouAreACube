using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube.Gameplay.Chips.ChipsList.DataGenerationChips
{
    [Serializable()]
    internal class GetSurroundingsChip : OutputPin<List<Tile>>
    {
        public override void Run(Block actor, UserInput input, ActionsList actions)
        {
            var neighbours = actor.Location.Adjacent.Values.ToList();
            SetOutput(neighbours);
        }
    }
}
