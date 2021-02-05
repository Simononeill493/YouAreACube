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
        public Point AbsoluteLocation;
        public Dictionary<CardinalDirection, TContents> Adjacent;

        public LocationWithNeighbors(Point location)
        {
            AbsoluteLocation = location;
            Adjacent = new Dictionary<CardinalDirection, TContents>();
        }

        public bool HasNeighbour(CardinalDirection direction)
        {
            return Adjacent.ContainsKey(direction);
        }

        public bool DirectionIsValid(CardinalDirection direction)
        {
            return Adjacent.ContainsKey(direction);
        }

        public override string ToString()
        {
            return "(" + AbsoluteLocation.X + " " + AbsoluteLocation.Y + ")" + " " + Adjacent.Count;
        }
    }
}
