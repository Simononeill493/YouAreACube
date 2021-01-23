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
        private static Direction[] _directions;

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
            _directions = typeof(Direction).GetEnumValues().Cast<Direction>().ToArray();
        }

        public static Direction RandomDirection()
        {
            return (Direction)R.Next(0, _numDirections);
        }
    }
}
