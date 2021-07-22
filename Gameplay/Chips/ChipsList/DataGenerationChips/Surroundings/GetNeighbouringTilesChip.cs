using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube.Gameplay.Chips.ChipsList.DataGenerationChips
{
    [Serializable()]
    internal class GetNeighbouringTilesChip : OutputPin<List<Tile>>
    {
        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            SetOutput(actor.Location.Neighbours);
        }
    }
}
