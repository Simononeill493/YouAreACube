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
        public string Name { get; set; }

        public List<Tile> Value { get; set; }

        public void Run(Cube actor, UserInput input, ActionsList actions)
        {
            Value = (actor.Location.Neighbours);
        }
    }
}
