using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube.Gameplay.Chips.ChipsList.DataGenerationChips
{
    [Serializable()]
    internal class GetAdjacentTilesChip : OutputPin<List<Tile>>
    {
        public override void Run(Block actor, UserInput input, ActionsList actions)
        {
            SetOutput(actor.Location.Neighbours);
        }
    }
}
