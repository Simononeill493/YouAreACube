using System;
using System.Collections.Generic;

namespace IAmACube
{
    [Serializable()]
    public class SectorEmmigrantsList
    {
        public List<(Cube Block, IntPoint SectorLocation)> Moved;
        public List<(Cube Block, IntPoint SectorLocation)> Created;

        public SectorEmmigrantsList()
        {
            Moved = new List<(Cube Block, IntPoint SectorLocation)>();
            Created = new List<(Cube Block, IntPoint SectorLocation)>();
        }

        public List<(Cube Block, IntPoint SectorLocation)> GetAll()
        {
            var output = new List<(Cube Block, IntPoint SectorLocation)>();
            output.AddRange(Moved);
            output.AddRange(Created);
            return output;
        }

        public void AddMoved(List<(Cube, IntPoint)> toAdd) => Moved.AddRange(toAdd);
        public void AddCreated(List<(Cube, IntPoint)> toAdd) => Created.AddRange(toAdd);
        public void AddList(SectorEmmigrantsList toAdd)
        {
            Moved.AddRange(toAdd.Moved);
            Created.AddRange(toAdd.Created);
        }
    }
}
