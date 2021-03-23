using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class RandomUtils
    {
        public static Random R;

        private const int _numDirections = 8;
        private static CardinalDirection[] _directions;

        public static void Init()
        {
            R = new Random();
            _setLookups();
        }
        public static void Init(int seed)
        {
            R = new Random(seed);
            _setLookups();
        }
        private static void _setLookups()
        {
            _directions = typeof(CardinalDirection).GetEnumValues().Cast<CardinalDirection>().ToArray();
        }

        public static CardinalDirection RandomDirection()
        {
            return (CardinalDirection)R.Next(0, _numDirections);
        }

        public static int RandomNumber(int upperBoundExclusive)
        {
            return R.Next(0, upperBoundExclusive);
        }

        public static List<T> Shuffle<T>(List<T> toShuffle)
        {
            return toShuffle.OrderBy(x => R.Next()).ToList();
        }

    }
}
