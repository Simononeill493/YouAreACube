using System;
using System.Collections.Generic;

namespace IAmACube
{
    [Serializable()]
    public class SectorEmmigrantsList
    {
        public List<(Block Block, IntPoint SectorLocation)> Moved;
        public List<(Block Block, IntPoint SectorLocation)> Created;

        public SectorEmmigrantsList()
        {
            Moved = new List<(Block Block, IntPoint SectorLocation)>();
            Created = new List<(Block Block, IntPoint SectorLocation)>();
        }

        public List<(Block Block, IntPoint SectorLocation)> GetAll()
        {
            var output = new List<(Block Block, IntPoint SectorLocation)>();
            output.AddRange(Moved);
            output.AddRange(Created);
            return output;
        }

        public void AddMoved(List<(Block, IntPoint)> toAdd) => Moved.AddRange(toAdd);
        public void AddCreated(List<(Block, IntPoint)> toAdd) => Created.AddRange(toAdd);
        public void AddList(SectorEmmigrantsList toAdd)
        {
            Moved.AddRange(toAdd.Moved);
            Created.AddRange(toAdd.Created);
        }
    }
}
