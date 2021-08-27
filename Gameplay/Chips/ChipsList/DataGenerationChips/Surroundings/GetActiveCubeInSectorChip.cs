using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class GetActiveCubeInSectorChip : OutputPin<Cube>
    {
        public string Name { get; set; }

        public Cube Value { get; set; }

        public void Run(Cube actor, UserInput input, ActionsList actions)
        {
            var sector = Game.World.GetSector(actor.Location.SectorID);
            Value = sector.ActiveBlocks.GetRandom();
        }
    }
}