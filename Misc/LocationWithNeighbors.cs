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
        public readonly IntPoint AbsoluteLocation;
        public readonly Dictionary<CardinalDirection, TContents> Adjacent;
        public List<TContents> Neighbours => Adjacent.Values.ToList();

        public LocationWithNeighbors(IntPoint location)
        {
            AbsoluteLocation = location;
            Adjacent = new Dictionary<CardinalDirection, TContents>();
        }

        public bool TryGetNeighbour(CardinalDirection direction, out TContents neighbour) => Adjacent.TryGetValue(direction, out neighbour);
        public bool HasNeighbour(CardinalDirection direction) => Adjacent.ContainsKey(direction);
        public bool DirectionIsValid(CardinalDirection direction) => HasNeighbour(direction);
        public IEnumerable<CardinalDirection> GetEmptyAdjacents() => DirectionUtils.Cardinals.Where(c => !Adjacent.ContainsKey(c));

        public override string ToString() => "(" + AbsoluteLocation.X + " " + AbsoluteLocation.Y + ")";
    }
}
