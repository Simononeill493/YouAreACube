using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class LocationWithNeighbors<TContents> where TContents : LocationWithNeighbors<TContents>
    {
        public IntPoint AbsoluteLocation;
        public Dictionary<CardinalDirection, TContents> Adjacent;

        public List<TContents> Neighbours => Adjacent.Values.ToList();
        public IEnumerable<CardinalDirection> EmptyAdjacents => DirectionUtils.Cardinals.Where(c => !Adjacent.ContainsKey(c));

        public LocationWithNeighbors(IntPoint location)
        {
            AbsoluteLocation = location;
            Adjacent = new Dictionary<CardinalDirection, TContents>();
        }

        public bool HasNeighbour(CardinalDirection direction) => Adjacent.ContainsKey(direction);
        public bool DirectionIsValid(CardinalDirection direction) => HasNeighbour(direction);
        public override string ToString() => "(" + AbsoluteLocation.X + " " + AbsoluteLocation.Y + ")";
    }
}
